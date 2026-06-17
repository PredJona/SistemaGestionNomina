using System;
using System.Collections.Generic;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository usuarioRepository = new UsuarioRepository();

        public Usuario Login(string username, string password)
        {
            Usuario usuario = usuarioRepository.GetByUsername(username);
            if (usuario == null || !PasswordHelper.Verify(password, usuario.PasswordHash))
            {
                return null;
            }

            return usuario;
        }
    }

    public class EmpleadoService
    {
        private readonly EmpleadoRepository empleadoRepository = new EmpleadoRepository();
        private readonly DepartamentoRepository departamentoRepository = new DepartamentoRepository();

        public List<Empleado> GetAll(string search, int? departmentId, string status)
        {
            return empleadoRepository.GetAll(search, departmentId, status);
        }

        public List<Empleado> GetActive(int? departmentId)
        {
            return empleadoRepository.GetActiveByDepartment(departmentId);
        }

        public Empleado GetById(int id)
        {
            return empleadoRepository.GetById(id);
        }

        public List<Departamento> GetDepartments()
        {
            return departamentoRepository.GetAll();
        }

        public int Save(Empleado empleado)
        {
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
                return empleadoRepository.Add(empleado);
            }

            empleadoRepository.Update(empleado);
            return empleado.IdEmpleado;
        }

        public void Deactivate(int id)
        {
            empleadoRepository.Deactivate(id);
        }
    }

    public class AsistenciaService
    {
        private readonly AsistenciaRepository asistenciaRepository = new AsistenciaRepository();

        public int Register(Asistencia asistencia)
        {
            if (asistencia.Estado == "Falta" || asistencia.Estado == "Permiso")
            {
                asistencia.HoraEntrada = null;
                asistencia.HoraSalida = null;
                asistencia.HorasTrabajadas = 0;
            }
            else if (asistencia.HoraEntrada.HasValue && asistencia.HoraSalida.HasValue)
            {
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

            return asistenciaRepository.Add(asistencia);
        }

        public List<Asistencia> GetAll(DateTime? start, DateTime? end, int? employeeId, string status)
        {
            return asistenciaRepository.GetAll(start, end, employeeId, status);
        }
    }

    public class ConfiguracionService
    {
        private readonly ConfiguracionRepository configuracionRepository = new ConfiguracionRepository();

        public List<ConfiguracionNomina> GetAll()
        {
            return configuracionRepository.GetAll();
        }

        public decimal GetValue(string name, decimal fallback)
        {
            return configuracionRepository.GetValue(name, fallback);
        }

        public void SaveDefaults(Dictionary<string, decimal> values)
        {
            configuracionRepository.Save("SeguroSocial", values["SeguroSocial"], "Porcentaje académico de seguro social.");
            configuracionRepository.Save("ISR", values["ISR"], "Porcentaje académico de impuesto sobre la renta.");
            configuracionRepository.Save("SeguroEducativo", values["SeguroEducativo"], "Porcentaje académico de seguro educativo.");
            configuracionRepository.Save("RecargoHoraExtra", values["RecargoHoraExtra"], "Multiplicador académico de horas extra.");
            configuracionRepository.Save("HorasMensualesBase", values["HorasMensualesBase"], "Horas mensuales base para cálculo por hora.");
        }
    }

    public class NominaService
    {
        private readonly EmpleadoRepository empleadoRepository = new EmpleadoRepository();
        private readonly AsistenciaRepository asistenciaRepository = new AsistenciaRepository();
        private readonly ConfiguracionRepository configuracionRepository = new ConfiguracionRepository();
        private readonly NominaRepository nominaRepository = new NominaRepository();
        private readonly ComprobanteRepository comprobanteRepository = new ComprobanteRepository();

        public Nomina CalcularNomina(DateTime fechaInicio, DateTime fechaFin, int? departamentoId)
        {
            if (fechaInicio.Date > fechaFin.Date)
            {
                throw new InvalidOperationException("La fecha de inicio debe ser menor o igual que la fecha fin.");
            }

            List<Empleado> empleados = empleadoRepository.GetActiveByDepartment(departamentoId);
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
            nomina.Estado = "Calculada";

            foreach (Empleado empleado in empleados)
            {
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

            return nomina;
        }

        public int ConfirmarPago(Nomina nomina, DateTime fechaInicio, DateTime fechaFin)
        {
            if (nomina == null || nomina.Detalles.Count == 0)
            {
                throw new InvalidOperationException("No hay una nómina calculada para confirmar.");
            }

            PeriodoNomina periodo = new PeriodoNomina();
            periodo.Nombre = nomina.PeriodoNombre;
            periodo.FechaInicio = fechaInicio;
            periodo.FechaFin = fechaFin;
            periodo.Estado = "Confirmado";
            int idPeriodo = nominaRepository.CreatePeriodo(periodo);

            nomina.IdPeriodo = idPeriodo;
            nomina.Estado = "Confirmada";
            int idNomina = nominaRepository.CreateNomina(nomina);

            for (int i = 0; i < nomina.Detalles.Count; i++)
            {
                NominaDetalle detalle = nomina.Detalles[i];
                Comprobante comprobante = new Comprobante();
                comprobante.IdNomina = idNomina;
                comprobante.IdEmpleado = detalle.IdEmpleado;
                comprobante.NumeroComprobante = "COMP-" + idNomina.ToString("0000") + "-" + detalle.IdEmpleado.ToString("0000");
                comprobante.FechaGeneracion = DateTime.Now;
                comprobante.RutaPdf = string.Empty;
                comprobanteRepository.Add(comprobante);
            }

            return idNomina;
        }

        public List<Nomina> GetNominas()
        {
            return nominaRepository.GetAll();
        }

        public List<NominaDetalle> GetDetalles(int idNomina)
        {
            return nominaRepository.GetDetalles(idNomina);
        }
    }

    public class ComprobanteService
    {
        private readonly ComprobanteRepository comprobanteRepository = new ComprobanteRepository();

        public List<Comprobante> GetAll(string search)
        {
            return comprobanteRepository.GetAll(search);
        }

        public Comprobante GetById(int id)
        {
            return comprobanteRepository.GetById(id);
        }

        public void SaveRutaPdf(int idComprobante, string ruta)
        {
            comprobanteRepository.UpdateRutaPdf(idComprobante, ruta);
        }
    }

    public class ReporteService
    {
        private readonly ReporteRepository reporteRepository = new ReporteRepository();

        public List<ReporteGenerado> GetAll()
        {
            return reporteRepository.GetAll();
        }

        public ReporteGenerado Register(string name, string type, string path)
        {
            ReporteGenerado reporte = new ReporteGenerado();
            reporte.NombreReporte = name;
            reporte.Tipo = type;
            reporte.GeneradoPor = "admin";
            reporte.FechaGeneracion = DateTime.Now;
            reporte.RutaArchivo = path;
            reporte.IdReporte = reporteRepository.Add(reporte);
            return reporte;
        }
    }
}
