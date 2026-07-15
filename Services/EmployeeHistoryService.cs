using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public sealed class EmployeeHistoryService
    {
        private readonly EmployeeHistoryRepository historyRepository = new EmployeeHistoryRepository();
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();
        private readonly NominaRepository payrollRepository = new NominaRepository();
        private readonly AuditRepository auditRepository = new AuditRepository();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();

        public List<HistorialEmpleado> Search(EmployeeHistoryQuery query)
        {
            authorizationService.DemandPermission(Permissions.EmployeeHistoryView);
            if (query != null && query.EmployeeId.HasValue)
                employeeScopeService.DemandEmployeeInScope(query.EmployeeId.Value);
            if (query != null && query.From.HasValue && query.To.HasValue && query.From.Value.Date > query.To.Value.Date)
                throw new ArgumentException("La fecha inicial no puede superar la fecha final.");
            return historyRepository.Search(query, employeeScopeService.GetDepartmentScope());
        }

        internal EmployeeSaveResult SaveEmployee(Empleado candidate, EmployeeChangeContext context)
        {
            Empleado original = employeeRepository.GetById(candidate.IdEmpleado);
            if (original == null) throw new InvalidOperationException("El empleado no existe.");
            employeeScopeService.DemandEmployeeInScope(candidate.IdEmpleado);

            List<HistorialEmpleado> changes = BuildChanges(original, candidate);
            if (changes.Count == 0)
            {
                candidate.FechaEfectivaLaboral = original.FechaEfectivaLaboral ?? original.FechaIngreso;
                employeeRepository.Update(candidate);
                return new EmployeeSaveResult
                {
                    EmployeeId = candidate.IdEmpleado,
                    AppliedImmediately = true
                };
            }

            if (context == null || string.IsNullOrWhiteSpace(context.Reason))
                throw new ArgumentException("Debe indicar la fecha efectiva y el motivo del cambio laboral.");

            DateTime effectiveDate = context.EffectiveDate.Date;
            if (effectiveDate == DateTime.MinValue.Date)
                throw new ArgumentException("La fecha efectiva del cambio no es válida.");
            if (effectiveDate < original.FechaIngreso.Date)
                throw new ArgumentException("La fecha efectiva no puede ser anterior al ingreso del empleado.");
            if (effectiveDate > DateTime.Today)
                authorizationService.DemandPermission(Permissions.EmployeeChangesSchedule);
            if (payrollRepository.HasClosedPeriodAffectedByEmployeeChange(effectiveDate))
                throw new InvalidOperationException("El cambio laboral afectaría un período de nómina cerrado.");

            bool applyNow = effectiveDate <= DateTime.Today;
            string user = SessionContext.IsAuthenticated ? SessionContext.Username : "sistema";
            DateTime now = DateTime.Now;
            for (int i = 0; i < changes.Count; i++)
            {
                changes[i].FechaCambio = now;
                changes[i].FechaEfectiva = effectiveDate;
                changes[i].UsuarioResponsable = user;
                changes[i].Motivo = context.Reason.Trim();
                changes[i].Aplicado = applyNow;
                changes[i].FechaAplicacion = applyNow ? (DateTime?)now : null;
            }

            Empleado valueToPersist = applyNow ? candidate : PreserveTrackedValues(candidate, original);
            valueToPersist.FechaEfectivaLaboral = applyNow
                ? effectiveDate : original.FechaEfectivaLaboral ?? original.FechaIngreso;

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < changes.Count; i++) historyRepository.Add(connection, transaction, changes[i]);
                    employeeRepository.Update(connection, transaction, valueToPersist);
                    auditRepository.Add(connection, transaction, new AuditRecord
                    {
                        Usuario = user,
                        Modulo = "Empleados",
                        Accion = applyNow ? "Aplicar cambio laboral" : "Programar cambio laboral",
                        Detalle = "IdEmpleado=" + candidate.IdEmpleado + ", Campos=" + changes.Count +
                            ", FechaEfectiva=" + effectiveDate.ToString("yyyy-MM-dd"),
                        Fecha = now
                    });
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new EmployeeSaveResult
            {
                EmployeeId = candidate.IdEmpleado,
                HistoryRecords = changes.Count,
                AppliedImmediately = applyNow,
                HasScheduledChanges = !applyNow
            };
        }

        public int ApplyDueChanges(DateTime effectiveThrough)
        {
            int applied = 0;
            DateTime now = DateTime.Now;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    List<HistorialEmpleado> pending = historyRepository.GetPendingDue(
                        connection, transaction, effectiveThrough.Date);
                    for (int i = 0; i < pending.Count; i++)
                    {
                        HistorialEmpleado item = pending[i];
                        if (payrollRepository.HasClosedPeriodAffectedByEmployeeChange(
                            connection, transaction, item.FechaEfectiva))
                        {
                            auditRepository.Add(connection, transaction, new AuditRecord
                            {
                                Usuario = "sistema",
                                Modulo = "Empleados",
                                Accion = "Cambio programado bloqueado",
                                Detalle = "IdHistorial=" + item.IdHistorial + ", período cerrado relacionado",
                                Fecha = now
                            });
                            continue;
                        }

                        historyRepository.ApplyChange(connection, transaction, item, now);
                        auditRepository.Add(connection, transaction, new AuditRecord
                        {
                            Usuario = "sistema",
                            Modulo = "Empleados",
                            Accion = "Aplicar cambio programado",
                            Detalle = "IdHistorial=" + item.IdHistorial + ", IdEmpleado=" + item.IdEmpleado,
                            Fecha = now
                        });
                        applied++;
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return applied;
        }

        private List<HistorialEmpleado> BuildChanges(Empleado original, Empleado candidate)
        {
            List<HistorialEmpleado> changes = new List<HistorialEmpleado>();
            if (original.SalarioBase != candidate.SalarioBase)
            {
                changes.Add(NewChange(candidate.IdEmpleado, EmployeeHistoryFields.BaseSalary,
                    "B/. " + original.SalarioBase.ToString("0.00"),
                    "B/. " + candidate.SalarioBase.ToString("0.00"),
                    original.SalarioBase.ToString(CultureInfo.InvariantCulture),
                    candidate.SalarioBase.ToString(CultureInfo.InvariantCulture)));
            }
            if (!string.Equals(original.Cargo, candidate.Cargo, StringComparison.Ordinal))
            {
                changes.Add(NewChange(candidate.IdEmpleado, EmployeeHistoryFields.Position,
                    original.Cargo, candidate.Cargo, original.Cargo, candidate.Cargo));
            }
            if (original.IdDepartamento != candidate.IdDepartamento)
            {
                changes.Add(NewChange(candidate.IdEmpleado, EmployeeHistoryFields.Department,
                    original.DepartamentoNombre, employeeRepository.GetDepartmentName(candidate.IdDepartamento),
                    original.IdDepartamento.ToString(CultureInfo.InvariantCulture),
                    candidate.IdDepartamento.ToString(CultureInfo.InvariantCulture)));
            }
            if (!string.Equals(original.Estado, candidate.Estado, StringComparison.OrdinalIgnoreCase))
            {
                changes.Add(NewChange(candidate.IdEmpleado, EmployeeHistoryFields.Status,
                    original.Estado, candidate.Estado, original.Estado, candidate.Estado));
            }
            return changes;
        }

        private static HistorialEmpleado NewChange(int employeeId, string field,
            string oldDisplay, string newDisplay, string oldTechnical, string newTechnical)
        {
            return new HistorialEmpleado
            {
                IdEmpleado = employeeId,
                CampoModificado = field,
                ValorAnterior = oldDisplay,
                ValorNuevo = newDisplay,
                ValorAnteriorTecnico = oldTechnical,
                ValorNuevoTecnico = newTechnical
            };
        }

        private static Empleado PreserveTrackedValues(Empleado candidate, Empleado original)
        {
            return new Empleado
            {
                IdEmpleado = candidate.IdEmpleado,
                Codigo = candidate.Codigo,
                Nombre = candidate.Nombre,
                Apellido = candidate.Apellido,
                Cedula = candidate.Cedula,
                Cargo = original.Cargo,
                IdDepartamento = original.IdDepartamento,
                DepartamentoNombre = original.DepartamentoNombre,
                SalarioBase = original.SalarioBase,
                Estado = original.Estado,
                FechaIngreso = candidate.FechaIngreso,
                FechaEfectivaLaboral = original.FechaEfectivaLaboral
            };
        }
    }

    public sealed class EmployeeEffectiveDataService
    {
        private readonly EmployeeHistoryRepository historyRepository = new EmployeeHistoryRepository();
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();
        private readonly EmployeeHistoryService historyService = new EmployeeHistoryService();

        public List<Empleado> GetActiveEmployeesAsOf(DateTime effectiveDate, int? departmentId)
        {
            historyService.ApplyDueChanges(DateTime.Today);
            List<Empleado> source = employeeRepository.GetAllForEffectiveDate(null);
            List<Empleado> result = new List<Empleado>();
            for (int i = 0; i < source.Count; i++)
            {
                Empleado resolved = ResolveAsOf(source[i], effectiveDate.Date);
                if (!string.Equals(resolved.Estado, "Activo", StringComparison.OrdinalIgnoreCase)) continue;
                if (departmentId.HasValue && resolved.IdDepartamento != departmentId.Value) continue;
                result.Add(resolved);
            }
            return result;
        }

        public Empleado GetEmployeeAsOf(int employeeId, DateTime effectiveDate)
        {
            historyService.ApplyDueChanges(DateTime.Today);
            Empleado employee = employeeRepository.GetById(employeeId);
            return employee == null ? null : ResolveAsOf(employee, effectiveDate.Date);
        }

        private Empleado ResolveAsOf(Empleado current, DateTime date)
        {
            Empleado result = Clone(current);
            List<HistorialEmpleado> history = historyRepository.GetByEmployee(current.IdEmpleado);

            for (int i = history.Count - 1; i >= 0; i--)
            {
                HistorialEmpleado item = history[i];
                if (item.Aplicado && item.FechaEfectiva.Date > date)
                    ApplyValue(result, item.CampoModificado, item.ValorAnteriorTecnico);
            }
            for (int i = 0; i < history.Count; i++)
            {
                HistorialEmpleado item = history[i];
                if (!item.Aplicado && item.FechaEfectiva.Date <= date)
                    ApplyValue(result, item.CampoModificado, item.ValorNuevoTecnico);
            }
            result.DepartamentoNombre = employeeRepository.GetDepartmentName(result.IdDepartamento);
            return result;
        }

        private static void ApplyValue(Empleado employee, string field, string value)
        {
            if (field == EmployeeHistoryFields.BaseSalary)
                employee.SalarioBase = decimal.Parse(value, CultureInfo.InvariantCulture);
            else if (field == EmployeeHistoryFields.Position) employee.Cargo = value;
            else if (field == EmployeeHistoryFields.Department)
                employee.IdDepartamento = int.Parse(value, CultureInfo.InvariantCulture);
            else if (field == EmployeeHistoryFields.Status) employee.Estado = value;
        }

        private static Empleado Clone(Empleado source)
        {
            return new Empleado
            {
                IdEmpleado = source.IdEmpleado,
                Codigo = source.Codigo,
                Nombre = source.Nombre,
                Apellido = source.Apellido,
                Cedula = source.Cedula,
                Cargo = source.Cargo,
                IdDepartamento = source.IdDepartamento,
                DepartamentoNombre = source.DepartamentoNombre,
                SalarioBase = source.SalarioBase,
                Estado = source.Estado,
                FechaIngreso = source.FechaIngreso,
                FechaEfectivaLaboral = source.FechaEfectivaLaboral
            };
        }
    }
}
