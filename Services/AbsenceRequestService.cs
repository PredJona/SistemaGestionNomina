using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public sealed class AbsenceRequestService
    {
        private readonly AbsenceRequestRepository repository = new AbsenceRequestRepository();
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();
        private readonly EmployeeEffectiveDataService effectiveEmployeeService = new EmployeeEffectiveDataService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly PayrollPeriodPolicyService periodPolicyService = new PayrollPeriodPolicyService();
        private readonly AuditRepository auditRepository = new AuditRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public List<SolicitudAusencia> Search(AbsenceQuery query)
        {
            query = query ?? new AbsenceQuery();
            ValidateQuery(query);
            int? forcedEmployeeId = null;
            int? scopeDepartmentId = null;
            if (IsWorker())
            {
                authorizationService.DemandPermission(Permissions.OwnAbsencesView);
                forcedEmployeeId = employeeScopeService.RequireCurrentEmployeeId();
                if (query.EmployeeId.HasValue) employeeScopeService.DemandCurrentEmployee(query.EmployeeId.Value);
            }
            else
            {
                authorizationService.DemandPermission(Permissions.AbsencesView);
                if (query.EmployeeId.HasValue) employeeScopeService.DemandEmployeeInScope(query.EmployeeId.Value);
                scopeDepartmentId = employeeScopeService.GetDepartmentScope();
            }

            List<SolicitudAusencia> items = repository.Search(query, forcedEmployeeId, scopeDepartmentId);
            auditTrailService.RegistrarAccion("Ausencias", "Consultar", "Registros=" + items.Count);
            return items;
        }

        public SolicitudAusencia GetById(int requestId)
        {
            if (requestId <= 0) throw new ArgumentException("Seleccione una solicitud válida.");
            SolicitudAusencia item = repository.GetById(requestId);
            if (item == null) throw new InvalidOperationException("La solicitud no existe.");
            DemandRequestScope(item, Permissions.AbsencesView, Permissions.OwnAbsencesView);
            return item;
        }

        public int CreateForEmployee(SolicitudAusencia item)
        {
            authorizationService.DemandPermission(Permissions.AbsencesCreate);
            if (item == null) throw new ArgumentNullException("item");
            employeeScopeService.DemandEmployeeInScope(item.IdEmpleado);
            return Create(item.IdEmpleado, item.Tipo, item.FechaInicio, item.FechaFin, item.Motivo);
        }

        public int CreateOwn(OwnAbsenceRequest request)
        {
            authorizationService.DemandPermission(Permissions.OwnAbsencesCreate);
            if (request == null) throw new ArgumentNullException("request");
            int employeeId = employeeScopeService.RequireCurrentEmployeeId();
            return Create(employeeId, request.Type, request.StartDate, request.EndDate, request.Reason);
        }

        public void Approve(int requestId, string observation)
        {
            authorizationService.DemandPermission(Permissions.AbsencesApprove);
            SolicitudAusencia current = repository.GetById(requestId);
            if (current == null) throw new InvalidOperationException("La solicitud no existe.");
            employeeScopeService.DemandEmployeeInScope(current.IdEmpleado);
            AbsenceStateMachine.DemandTransition(current.Estado, AbsenceStates.Approved);
            periodPolicyService.VerificarFechasAbiertas(current.FechaInicio, current.FechaFin);

            List<DateTime> workdays = GetWorkdays(current.FechaInicio, current.FechaFin);
            if (workdays.Count == 0) throw new InvalidOperationException("La solicitud no contiene días laborables de lunes a viernes.");
            string user = CurrentUsername();
            DateTime now = DateTime.Now;

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    current = repository.GetById(connection, transaction, requestId);
                    if (current == null) throw new InvalidOperationException("La solicitud no existe.");
                    AbsenceStateMachine.DemandTransition(current.Estado, AbsenceStates.Approved);
                    if (repository.HasApprovedOverlap(connection, transaction, current.IdEmpleado,
                        current.FechaInicio, current.FechaFin, current.IdSolicitud))
                        throw new InvalidOperationException("Ya existe una ausencia aprobada que se solapa con el rango.");

                    // Se valida todo el rango antes de escribir para no reemplazar asistencias existentes.
                    for (int i = 0; i < workdays.Count; i++)
                    {
                        if (repository.AttendanceExists(connection, transaction, current.IdEmpleado, workdays[i]))
                            throw new InvalidOperationException("Ya existe asistencia para " +
                                workdays[i].ToString("dd/MM/yyyy") + ". No se reemplazó ningún registro.");
                    }

                    repository.UpdateState(connection, transaction, requestId, AbsenceStates.Pending,
                        AbsenceStates.Approved, user, now, observation);
                    string attendanceStatus = AbsenceTypes.ToAttendanceStatus(current.Tipo);
                    // La aprobacion refleja la ausencia como asistencia y mantiene el vinculo para poder revertirla.
                    for (int i = 0; i < workdays.Count; i++)
                        repository.AddGeneratedAttendance(connection, transaction, requestId,
                            current.IdEmpleado, workdays[i], attendanceStatus);
                    AddAudit(connection, transaction, user, "Aprobar",
                        "IdSolicitud=" + requestId + ", Días=" + workdays.Count, now);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Reject(int requestId, string observation)
        {
            authorizationService.DemandPermission(Permissions.AbsencesReject);
            if (string.IsNullOrWhiteSpace(observation))
                throw new ArgumentException("Debe indicar la razón del rechazo.");
            SolicitudAusencia current = repository.GetById(requestId);
            if (current == null) throw new InvalidOperationException("La solicitud no existe.");
            employeeScopeService.DemandEmployeeInScope(current.IdEmpleado);
            AbsenceStateMachine.DemandTransition(current.Estado, AbsenceStates.Rejected);
            periodPolicyService.VerificarFechasAbiertas(current.FechaInicio, current.FechaFin);
            ChangeState(current, AbsenceStates.Rejected, observation.Trim(), "Rechazar");
        }

        public void Cancel(int requestId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Debe indicar el motivo de cancelación.");
            SolicitudAusencia current = repository.GetById(requestId);
            if (current == null) throw new InvalidOperationException("La solicitud no existe.");

            if (IsWorker())
            {
                authorizationService.DemandPermission(Permissions.OwnAbsencesCancel);
                employeeScopeService.DemandCurrentEmployee(current.IdEmpleado);
                if (!string.Equals(current.Estado, AbsenceStates.Pending, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("El trabajador solo puede cancelar solicitudes propias pendientes.");
            }
            else
            {
                authorizationService.DemandPermission(Permissions.AbsencesCancel);
                employeeScopeService.DemandEmployeeInScope(current.IdEmpleado);
            }

            AbsenceStateMachine.DemandTransition(current.Estado, AbsenceStates.Cancelled);
            periodPolicyService.VerificarFechasAbiertas(current.FechaInicio, current.FechaFin);
            string user = CurrentUsername();
            DateTime now = DateTime.Now;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    if (string.Equals(current.Estado, AbsenceStates.Approved, StringComparison.OrdinalIgnoreCase))
                        // Solo se eliminan las asistencias creadas automaticamente por esta solicitud.
                        repository.DeleteGeneratedAttendance(connection, transaction, current.IdSolicitud);
                    repository.UpdateState(connection, transaction, current.IdSolicitud, current.Estado,
                        AbsenceStates.Cancelled, user, now, reason.Trim());
                    AddAudit(connection, transaction, user, "Cancelar",
                        "IdSolicitud=" + current.IdSolicitud + ", EstadoAnterior=" + current.Estado, now);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private int Create(int employeeId, string type, DateTime start, DateTime end, string reason)
        {
            ValidateRequest(employeeId, type, start, end, reason);
            Empleado employee = employeeRepository.GetById(employeeId);
            Empleado effectiveEmployee = effectiveEmployeeService.GetEmployeeAsOf(employeeId, start.Date);
            if (employee == null || !string.Equals(employee.Estado, "Activo", StringComparison.OrdinalIgnoreCase) ||
                effectiveEmployee == null || !string.Equals(effectiveEmployee.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El empleado debe estar activo al crear la solicitud y en su fecha inicial.");

            periodPolicyService.VerificarFechasAbiertas(start.Date, end.Date);
            string user = CurrentUsername();
            DateTime now = DateTime.Now;
            SolicitudAusencia item = new SolicitudAusencia
            {
                IdEmpleado = employeeId,
                Tipo = NormalizeType(type),
                FechaInicio = start.Date,
                FechaFin = end.Date,
                Estado = AbsenceStates.Pending,
                Motivo = reason.Trim(),
                UsuarioSolicitante = user,
                FechaCreacion = now
            };

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int id = repository.Add(connection, transaction, item);
                    AddAudit(connection, transaction, user, "Crear",
                        "IdSolicitud=" + id + ", IdEmpleado=" + employeeId + ", Tipo=" + item.Tipo, now);
                    transaction.Commit();
                    return id;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void ChangeState(SolicitudAusencia current, string newState, string observation, string action)
        {
            string user = CurrentUsername();
            DateTime now = DateTime.Now;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    repository.UpdateState(connection, transaction, current.IdSolicitud, current.Estado,
                        newState, user, now, observation);
                    AddAudit(connection, transaction, user, action, "IdSolicitud=" + current.IdSolicitud, now);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void DemandRequestScope(SolicitudAusencia item, string generalPermission, string ownPermission)
        {
            if (IsWorker())
            {
                authorizationService.DemandPermission(ownPermission);
                employeeScopeService.DemandCurrentEmployee(item.IdEmpleado);
            }
            else
            {
                authorizationService.DemandPermission(generalPermission);
                employeeScopeService.DemandEmployeeInScope(item.IdEmpleado);
            }
        }

        private static void ValidateQuery(AbsenceQuery query)
        {
            if (query.From.HasValue && query.To.HasValue && query.From.Value.Date > query.To.Value.Date)
                throw new ArgumentException("La fecha inicial no puede superar la fecha final.");
            if (!string.IsNullOrWhiteSpace(query.Type) && !AbsenceTypes.IsValid(query.Type))
                throw new ArgumentException("El tipo de ausencia no es válido.");
            if (!string.IsNullOrWhiteSpace(query.Status) && !AbsenceStates.IsValid(query.Status))
                throw new ArgumentException("El estado de ausencia no es válido.");
        }

        private static void ValidateRequest(int employeeId, string type, DateTime start, DateTime end, string reason)
        {
            if (employeeId <= 0) throw new ArgumentException("Debe seleccionar un empleado.");
            if (!AbsenceTypes.IsValid(type)) throw new ArgumentException("Seleccione un tipo de ausencia válido.");
            if (start.Date > end.Date) throw new ArgumentException("La fecha inicial no puede superar la fecha final.");
            if (GetWorkdays(start, end).Count == 0)
                throw new ArgumentException("La solicitud debe incluir al menos un día de lunes a viernes.");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Debe indicar el motivo de la solicitud.");
        }

        private static string NormalizeType(string type)
        {
            for (int i = 0; i < AbsenceTypes.All.Length; i++)
            {
                if (string.Equals(AbsenceTypes.All[i], type, StringComparison.OrdinalIgnoreCase))
                    return AbsenceTypes.All[i];
            }
            return type;
        }

        private static List<DateTime> GetWorkdays(DateTime start, DateTime end)
        {
            List<DateTime> dates = new List<DateTime>();
            for (DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    dates.Add(date);
            }
            return dates;
        }

        private void AddAudit(SQLiteConnection connection, SQLiteTransaction transaction, string user,
            string action, string detail, DateTime date)
        {
            auditRepository.Add(connection, transaction, new AuditRecord
            {
                Usuario = user,
                Modulo = "Ausencias",
                Accion = action,
                Detalle = detail,
                Fecha = date
            });
        }

        private static bool IsWorker()
        {
            return string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase);
        }

        private static string CurrentUsername()
        {
            if (!SessionContext.IsAuthenticated || string.IsNullOrWhiteSpace(SessionContext.Username))
                throw new UnauthorizedAccessException("Debe iniciar sesión para gestionar solicitudes.");
            return SessionContext.Username;
        }
    }
}
