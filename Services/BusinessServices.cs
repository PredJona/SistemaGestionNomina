using System;
using System.Collections.Generic;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public class AuthService
    {
        private const int MaximumAttempts = 5;
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(15);
        private readonly AuthenticationRepository authenticationRepository = new AuthenticationRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public AuthenticationResult Authenticate(string username, string password)
        {
            string normalizedUsername = (username ?? string.Empty).Trim();
            if (normalizedUsername.Length == 0 || string.IsNullOrEmpty(password))
            {
                return AuthenticationResult.Invalid();
            }

            Usuario usuario = authenticationRepository.GetByUsername(normalizedUsername);
            if (usuario == null || !string.Equals(usuario.Estado, "Activo", StringComparison.OrdinalIgnoreCase) ||
                !Roles.IsValid(usuario.Rol))
            {
                auditTrailService.RegistrarCambio(normalizedUsername, "Seguridad", "Inicio de sesión fallido",
                    "Credenciales no válidas.");
                return AuthenticationResult.Invalid();
            }

            DateTime now = DateTime.Now;
            if (usuario.Bloqueado && usuario.FechaBloqueo.HasValue)
            {
                DateTime blockedUntil = usuario.FechaBloqueo.Value.Add(LockDuration);
                if (now < blockedUntil)
                {
                    auditTrailService.RegistrarCambio(normalizedUsername, "Seguridad", "Acceso bloqueado",
                        "Intento durante bloqueo temporal.");
                    return AuthenticationResult.Blocked(blockedUntil);
                }

                authenticationRepository.RegisterFailedAttempt(usuario.IdUsuario, 0, false, null);
                usuario.IntentosFallidos = 0;
                usuario.Bloqueado = false;
                usuario.FechaBloqueo = null;
            }

            if (!PasswordHelper.Verify(password, usuario.PasswordHash))
            {
                int attempts = usuario.IntentosFallidos + 1;
                bool blocked = attempts >= MaximumAttempts;
                DateTime? blockedAt = blocked ? now : (DateTime?)null;
                authenticationRepository.RegisterFailedAttempt(usuario.IdUsuario, attempts, blocked, blockedAt);
                auditTrailService.RegistrarCambio(normalizedUsername, "Seguridad",
                    blocked ? "Usuario bloqueado" : "Inicio de sesión fallido",
                    blocked ? "Se alcanzó el máximo de intentos." : "Intento " + attempts + " de " + MaximumAttempts + ".");
                return blocked ? AuthenticationResult.Blocked(now.Add(LockDuration)) : AuthenticationResult.Invalid();
            }

            bool upgradeLegacyHash = PasswordHelper.IsLegacyHash(usuario.PasswordHash);
            string upgradedHash = upgradeLegacyHash ? PasswordHelper.HashPassword(password) : string.Empty;
            authenticationRepository.CompleteLogin(usuario.IdUsuario, now, upgradedHash);
            if (upgradeLegacyHash) usuario.PasswordHash = upgradedHash;
            usuario.UltimoAcceso = now;
            usuario.IntentosFallidos = 0;
            usuario.Bloqueado = false;
            usuario.FechaBloqueo = null;
            SessionContext.Begin(usuario);
            auditTrailService.RegistrarCambio(usuario.NombreUsuario, "Seguridad", "Inicio de sesión correcto",
                upgradeLegacyHash ? "Contraseña actualizada a PBKDF2." : string.Empty);
            return AuthenticationResult.Successful(usuario);
        }
    }

    public class EmpleadoService
    {
        private readonly EmpleadoRepository empleadoRepository = new EmpleadoRepository();
        private readonly DepartamentoRepository departamentoRepository = new DepartamentoRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();
        private readonly EmployeeHistoryService employeeHistoryService = new EmployeeHistoryService();

        public List<Empleado> GetAll(string search, int? departmentId, string status)
        {
            authorizationService.DemandPermission(Permissions.EmployeesView);
            employeeHistoryService.ApplyDueChanges(DateTime.Today);
            return empleadoRepository.GetAll(search, departmentId, status, employeeScopeService.GetDepartmentScope());
        }

        public List<Empleado> GetActive(int? departmentId)
        {
            authorizationService.DemandAny(Permissions.EmployeesView, Permissions.AttendanceView,
                Permissions.PayrollView, Permissions.ReportsPersonal);
            employeeHistoryService.ApplyDueChanges(DateTime.Today);
            int? scope = employeeScopeService.GetDepartmentScope();
            return empleadoRepository.GetActiveByDepartment(scope ?? departmentId, scope);
        }

        public Empleado GetById(int id)
        {
            authorizationService.DemandPermission(Permissions.EmployeesView);
            employeeScopeService.DemandEmployeeInScope(id);
            employeeHistoryService.ApplyDueChanges(DateTime.Today);
            return empleadoRepository.GetById(id);
        }

        public List<Departamento> GetDepartments()
        {
            authorizationService.DemandAny(Permissions.EmployeesView, Permissions.AttendanceView,
                Permissions.PayrollView);
            return departamentoRepository.GetAll();
        }

        public int Save(Empleado empleado)
        {
            return Save(empleado, null).EmployeeId;
        }

        public EmployeeSaveResult Save(Empleado empleado, EmployeeChangeContext changeContext)
        {
            if (empleado == null)
            {
                throw new ArgumentException("Debe indicar los datos del empleado.");
            }

            authorizationService.DemandPermission(empleado.IdEmpleado == 0
                ? Permissions.EmployeesCreate : Permissions.EmployeesEdit);

            if (empleado.IdDepartamento <= 0)
            {
                throw new ArgumentException("Debe seleccionar un departamento válido.");
            }

            if (empleado.SalarioBase <= 0)
            {
                throw new ArgumentException("El salario base debe ser mayor que cero.");
            }

            if (empleadoRepository.ExistsCode(empleado.Codigo, empleado.IdEmpleado))
            {
                throw new InvalidOperationException("Ya existe un empleado con ese código.");
            }

            if (empleadoRepository.ExistsCedula(empleado.Cedula, empleado.IdEmpleado))
            {
                throw new InvalidOperationException("Ya existe un empleado con esa cédula.");
            }

            if (empleado.IdEmpleado == 0)
            {
                empleado.FechaEfectivaLaboral = empleado.FechaIngreso;
                int id = empleadoRepository.Add(empleado);
                auditTrailService.RegistrarAccion("Empleados", "Crear", empleado.Codigo);
                return new EmployeeSaveResult
                {
                    EmployeeId = id,
                    AppliedImmediately = true
                };
            }

            EmployeeSaveResult result = employeeHistoryService.SaveEmployee(empleado, changeContext);
            if (result.HistoryRecords == 0)
                auditTrailService.RegistrarAccion("Empleados", "Actualizar", empleado.Codigo);
            return result;
        }

        public void Deactivate(int id)
        {
            throw new ArgumentException("Debe indicar la fecha efectiva y el motivo de desactivación.");
        }

        public EmployeeSaveResult Deactivate(int id, EmployeeChangeContext changeContext)
        {
            authorizationService.DemandPermission(Permissions.EmployeesDeactivate);
            Empleado employee = empleadoRepository.GetById(id);
            if (employee == null) throw new InvalidOperationException("El empleado no existe.");
            employee.Estado = "Inactivo";
            return employeeHistoryService.SaveEmployee(employee, changeContext);
        }
    }

    public class AsistenciaService
    {
        private readonly AsistenciaRepository asistenciaRepository = new AsistenciaRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly PayrollPeriodPolicyService periodPolicyService = new PayrollPeriodPolicyService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();

        public int Register(Asistencia asistencia)
        {
            if (asistencia == null)
            {
                throw new ArgumentException("Debe indicar los datos de asistencia.");
            }

            authorizationService.DemandPermission(Permissions.AttendanceRegister);
            employeeScopeService.DemandEmployeeInScope(asistencia.IdEmpleado);
            periodPolicyService.VerificarFechasAbiertas(asistencia.Fecha.Date, asistencia.Fecha.Date);

            if (asistenciaRepository.Exists(asistencia.IdEmpleado, asistencia.Fecha.Date))
            {
                throw new InvalidOperationException("Ya existe una asistencia para el empleado en esa fecha.");
            }

            if (asistencia.Estado == "Falta" || asistencia.Estado == "Permiso")
            {
                asistencia.HoraEntrada = null;
                asistencia.HoraSalida = null;
                asistencia.HorasTrabajadas = 0;
            }
            else if (asistencia.HoraEntrada.HasValue && asistencia.HoraSalida.HasValue)
            {
                // Para asistencia valida, la salida debe cerrar una jornada real.
                if (asistencia.HoraSalida.Value <= asistencia.HoraEntrada.Value)
                {
                    throw new InvalidOperationException("La hora de salida debe ser mayor que la hora de entrada.");
                }

                asistencia.HorasTrabajadas = Convert.ToDecimal((asistencia.HoraSalida.Value - asistencia.HoraEntrada.Value).TotalHours);
            }
            else
            {
                throw new InvalidOperationException("Debe registrar hora de entrada y salida.");
            }

            int id = asistenciaRepository.Add(asistencia);
            auditTrailService.RegistrarAccion("Asistencia", "Registrar",
                "IdEmpleado=" + asistencia.IdEmpleado + ", Fecha=" + asistencia.Fecha.ToString("yyyy-MM-dd"));
            return id;
        }

        public List<Asistencia> GetAll(DateTime? start, DateTime? end, int? employeeId, string status)
        {
            authorizationService.DemandPermission(Permissions.AttendanceView);
            if (start.HasValue && end.HasValue && start.Value.Date > end.Value.Date)
            {
                throw new ArgumentException("La fecha inicial no puede ser mayor que la fecha final.");
            }

            if (employeeId.HasValue) employeeScopeService.DemandEmployeeInScope(employeeId.Value);
            return asistenciaRepository.GetAll(start, end, employeeId, status,
                employeeScopeService.GetDepartmentScope());
        }
    }

    public class ConfiguracionService
    {
        private readonly ConfiguracionRepository configuracionRepository = new ConfiguracionRepository();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public List<ConfiguracionNomina> GetAll()
        {
            authorizationService.DemandPermission(Permissions.ConfigurationView);
            return configuracionRepository.GetAll();
        }

        public decimal GetValue(string name, decimal fallback)
        {
            authorizationService.DemandPermission(Permissions.ConfigurationView);
            return configuracionRepository.GetValue(name, fallback);
        }

        public void SaveDefaults(Dictionary<string, decimal> values)
        {
            authorizationService.DemandPermission(Permissions.ConfigurationEdit);
            if (values == null || !values.ContainsKey("SeguroSocial") || !values.ContainsKey("ISR") ||
                !values.ContainsKey("SeguroEducativo") || !values.ContainsKey("RecargoHoraExtra") ||
                !values.ContainsKey("HorasMensualesBase"))
            {
                throw new ArgumentException("La configuración de nómina está incompleta.");
            }
            configuracionRepository.Save("SeguroSocial", values["SeguroSocial"], "Porcentaje académico de seguro social.");
            configuracionRepository.Save("ISR", values["ISR"], "Porcentaje académico de impuesto sobre la renta.");
            configuracionRepository.Save("SeguroEducativo", values["SeguroEducativo"], "Porcentaje académico de seguro educativo.");
            configuracionRepository.Save("RecargoHoraExtra", values["RecargoHoraExtra"], "Multiplicador académico de horas extra.");
            configuracionRepository.Save("HorasMensualesBase", values["HorasMensualesBase"], "Horas mensuales base para cálculo por hora.");
            auditTrailService.RegistrarAccion("Configuración", "Actualizar", "Parámetros de nómina.");
        }
    }

    public class NominaService
    {
        private readonly EmpleadoRepository empleadoRepository = new EmpleadoRepository();
        private readonly AsistenciaRepository asistenciaRepository = new AsistenciaRepository();
        private readonly ConfiguracionRepository configuracionRepository = new ConfiguracionRepository();
        private readonly NominaRepository nominaRepository = new NominaRepository();
        private readonly ComprobanteRepository comprobanteRepository = new ComprobanteRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly EmployeeEffectiveDataService effectiveEmployeeService = new EmployeeEffectiveDataService();
        private readonly PayrollPeriodPolicyService periodPolicyService = new PayrollPeriodPolicyService();

        public Nomina CalcularNomina(DateTime fechaInicio, DateTime fechaFin, int? departamentoId)
        {
            authorizationService.DemandPermission(Permissions.PayrollCalculate);
            if (fechaInicio.Date > fechaFin.Date)
            {
                throw new InvalidOperationException("La fecha de inicio debe ser menor o igual que la fecha fin.");
            }

            periodPolicyService.VerificarFechasAbiertas(fechaInicio.Date, fechaFin.Date);

            List<Empleado> empleados = effectiveEmployeeService.GetActiveEmployeesAsOf(fechaFin.Date, departamentoId);
            if (empleados.Count == 0)
            {
                throw new InvalidOperationException("No hay empleados activos para calcular la nómina.");
            }

            decimal seguroSocial = configuracionRepository.GetValue("SeguroSocial", 9.75m);
            decimal isr = configuracionRepository.GetValue("ISR", 10m);
            decimal seguroEducativo = configuracionRepository.GetValue("SeguroEducativo", 1.25m);
            decimal recargoHoraExtra = configuracionRepository.GetValue("RecargoHoraExtra", 1.25m);
            decimal horasMensualesBase = configuracionRepository.GetValue("HorasMensualesBase", 160m);

            Nomina nomina = new Nomina();
            nomina.FechaCalculo = DateTime.Now;
            nomina.PeriodoNombre = "Nómina " + fechaInicio.ToString("dd/MM/yyyy") + " - " + fechaFin.ToString("dd/MM/yyyy");
            nomina.Estado = PayrollStates.Calculated;

            foreach (Empleado empleado in empleados)
            {
                // Formula academica: sueldo base + bono + horas extra - deducciones configurables.
                decimal horasExtra = asistenciaRepository.GetExtraHours(empleado.IdEmpleado, fechaInicio, fechaFin);
                decimal pagoHora = horasMensualesBase <= 0 ? 0 : empleado.SalarioBase / horasMensualesBase;
                decimal montoHorasExtra = Math.Round(horasExtra * pagoHora * recargoHoraExtra, 2);
                decimal bonos = Math.Round(empleado.SalarioBase * 0.03m, 2);
                decimal totalIngresos = empleado.SalarioBase + bonos + montoHorasExtra;
                decimal totalDeducciones = Math.Round(
                    totalIngresos * (seguroSocial / 100m) +
                    totalIngresos * (isr / 100m) +
                    totalIngresos * (seguroEducativo / 100m), 2);
                decimal neto = totalIngresos - totalDeducciones;

                NominaDetalle detalle = new NominaDetalle();
                detalle.IdEmpleado = empleado.IdEmpleado;
                detalle.CodigoEmpleado = empleado.Codigo;
                detalle.EmpleadoNombre = empleado.NombreCompleto;
                detalle.CargoEmpleado = empleado.Cargo;
                detalle.Departamento = empleado.DepartamentoNombre;
                detalle.SueldoBase = empleado.SalarioBase;
                detalle.Bonos = bonos;
                detalle.HorasExtra = horasExtra;
                detalle.MontoHorasExtra = montoHorasExtra;
                detalle.TotalIngresos = totalIngresos;
                detalle.TotalDeducciones = totalDeducciones;
                detalle.NetoPagar = neto;
                nomina.Detalles.Add(detalle);

                nomina.TotalIngresos += totalIngresos;
                nomina.TotalDeducciones += totalDeducciones;
                nomina.TotalNeto += neto;
            }

            auditTrailService.RegistrarAccion("Nómina", "Calcular",
                fechaInicio.ToString("yyyy-MM-dd") + " a " + fechaFin.ToString("yyyy-MM-dd") +
                ", Empleados=" + nomina.Detalles.Count);

            return nomina;
        }

        private readonly PayrollLifecycleService payrollLifecycleService = new PayrollLifecycleService();

        public int ConfirmarPago(Nomina nomina, DateTime fechaInicio, DateTime fechaFin)
        {
            return payrollLifecycleService.Confirmar(nomina, fechaInicio, fechaFin);
        }

        public void PagarNomina(int idNomina)
        {
            payrollLifecycleService.Pagar(idNomina);
        }

        public void AnularNomina(int idNomina, string motivo)
        {
            payrollLifecycleService.Anular(idNomina, motivo);
        }

        public int RecalcularNomina(int idNominaAnulada, DateTime fechaInicio, DateTime fechaFin, Nomina nominaRecalculada)
        {
            return payrollLifecycleService.Recalcular(idNominaAnulada, fechaInicio, fechaFin, nominaRecalculada);
        }

        public List<NominaVersion> ObtenerHistorialNomina(int idNomina)
        {
            return payrollLifecycleService.ObtenerHistorial(idNomina);
        }

        public List<Nomina> GetNominas()
        {
            authorizationService.DemandPermission(Permissions.PayrollView);
            return nominaRepository.GetAll();
        }

        public List<NominaDetalle> GetDetalles(int idNomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollView);
            return nominaRepository.GetDetalles(idNomina);
        }
    }

    public class ComprobanteService
    {
        private readonly ComprobanteRepository comprobanteRepository = new ComprobanteRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();

        public List<Comprobante> GetAll(string search)
        {
            authorizationService.DemandPermission(Permissions.PayslipsView);
            return comprobanteRepository.GetAll(search);
        }

        public Comprobante GetById(int id)
        {
            authorizationService.DemandPermission(Permissions.PayslipsView);
            return comprobanteRepository.GetById(id);
        }

        public void SaveRutaPdf(int idComprobante, string ruta)
        {
            authorizationService.DemandPermission(Permissions.PayslipsExport);
            comprobanteRepository.UpdateRutaPdf(idComprobante, ruta);
            auditTrailService.RegistrarAccion("Comprobantes", "Guardar PDF",
                "IdComprobante=" + idComprobante);
        }
    }

    public class ReporteService
    {
        private readonly ReporteRepository reporteRepository = new ReporteRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();

        public List<ReporteGenerado> GetAll()
        {
            authorizationService.DemandPermission(Permissions.ReportsView);
            List<ReporteGenerado> reports = reporteRepository.GetAll();
            if (string.Equals(SessionContext.Role, Roles.Admin, StringComparison.OrdinalIgnoreCase)) return reports;

            List<ReporteGenerado> filtered = new List<ReporteGenerado>();
            bool personalOnly = authorizationService.HasPermission(Permissions.ReportsPersonal) &&
                !authorizationService.HasPermission(Permissions.ReportsFinancial);
            for (int i = 0; i < reports.Count; i++)
            {
                bool isPersonal = (reports[i].Tipo ?? string.Empty).IndexOf("Personal",
                    StringComparison.OrdinalIgnoreCase) >= 0;
                if ((personalOnly && isPersonal) || (!personalOnly && !isPersonal)) filtered.Add(reports[i]);
            }
            return filtered;
        }

        public ReporteGenerado Register(string name, string type, string path)
        {
            authorizationService.DemandPermission(Permissions.ReportsExport);
            ReporteGenerado reporte = new ReporteGenerado();
            reporte.NombreReporte = name;
            reporte.Tipo = type;
            reporte.GeneradoPor = SessionContext.Username;
            reporte.FechaGeneracion = DateTime.Now;
            reporte.RutaArchivo = path;
            reporte.IdReporte = reporteRepository.Add(reporte);
            auditTrailService.RegistrarAccion("Reportes", "Generar", name + " - " + type);
            return reporte;
        }
    }
}
