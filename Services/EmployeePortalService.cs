using System;
using System.Collections.Generic;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public sealed class EmployeePortalService
    {
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();
        private readonly AsistenciaRepository attendanceRepository = new AsistenciaRepository();
        private readonly ComprobanteRepository payslipRepository = new ComprobanteRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public EmployeePortalDashboardViewModel GetDashboard()
        {
            authorizationService.DemandPermission(Permissions.PortalView);
            EmployeeProfileViewModel profile = GetProfileCore();
            DateTime today = DateTime.Today;
            List<Asistencia> monthAttendance = attendanceRepository.GetByEmployee(
                profile.IdEmpleado, new DateTime(today.Year, today.Month, 1), today, string.Empty);
            List<Asistencia> recent = new List<Asistencia>();
            for (int i = 0; i < monthAttendance.Count && i < 5; i++) recent.Add(monthAttendance[i]);

            List<Comprobante> payslips = payslipRepository.GetByEmployee(profile.IdEmpleado, string.Empty);
            auditTrailService.RegistrarAccion("Portal trabajador", "Consultar inicio", "Empleado=" + profile.IdEmpleado);
            return new EmployeePortalDashboardViewModel
            {
                Profile = profile,
                LatestPayslip = payslips.Count > 0 ? payslips[0] : null,
                RecentAttendance = recent,
                AttendanceSummary = SummarizeAttendance(monthAttendance)
            };
        }

        public EmployeeProfileViewModel GetMyProfile()
        {
            authorizationService.DemandPermission(Permissions.OwnProfileView);
            EmployeeProfileViewModel profile = GetProfileCore();
            auditTrailService.RegistrarAccion("Portal trabajador", "Consultar perfil", "Empleado=" + profile.IdEmpleado);
            return profile;
        }

        public List<Asistencia> GetMyAttendance(DateTime? start, DateTime? end, string status)
        {
            authorizationService.DemandPermission(Permissions.OwnAttendanceView);
            if (start.HasValue && end.HasValue && start.Value.Date > end.Value.Date)
            {
                throw new ArgumentException("La fecha inicial no puede ser posterior a la fecha final.");
            }

            string normalizedStatus = NormalizeAttendanceStatus(status);
            int employeeId = employeeScopeService.RequireCurrentEmployeeId();
            List<Asistencia> items = attendanceRepository.GetByEmployee(employeeId, start, end, normalizedStatus);
            auditTrailService.RegistrarAccion("Portal trabajador", "Consultar asistencia",
                "Empleado=" + employeeId + ", Registros=" + items.Count);
            return items;
        }

        public List<Comprobante> GetMyPayslips(string search)
        {
            authorizationService.DemandPermission(Permissions.OwnPayslipsView);
            int employeeId = employeeScopeService.RequireCurrentEmployeeId();
            List<Comprobante> items = payslipRepository.GetByEmployee(employeeId, (search ?? string.Empty).Trim());
            auditTrailService.RegistrarAccion("Portal trabajador", "Consultar comprobantes",
                "Empleado=" + employeeId + ", Registros=" + items.Count);
            return items;
        }

        public Comprobante GetMyPayslipById(int payslipId)
        {
            authorizationService.DemandPermission(Permissions.OwnPayslipsView);
            if (payslipId <= 0) throw new ArgumentException("Seleccione un comprobante valido.");

            int employeeId = employeeScopeService.RequireCurrentEmployeeId();
            Comprobante payslip = payslipRepository.GetByIdAndEmployee(payslipId, employeeId);
            if (payslip == null)
            {
                auditTrailService.RegistrarAccion("Portal trabajador", "Acceso rechazado a comprobante",
                    "IdComprobante=" + payslipId);
                throw new UnauthorizedAccessException("El comprobante no esta disponible para su cuenta.");
            }

            auditTrailService.RegistrarAccion("Portal trabajador", "Consultar comprobante",
                "IdComprobante=" + payslipId);
            return payslip;
        }

        public static EmployeeAttendanceSummaryViewModel SummarizeAttendance(IEnumerable<Asistencia> attendance)
        {
            EmployeeAttendanceSummaryViewModel summary = new EmployeeAttendanceSummaryViewModel();
            if (attendance == null) return summary;

            foreach (Asistencia item in attendance)
            {
                if (string.Equals(item.Estado, "Puntual", StringComparison.OrdinalIgnoreCase)) summary.Puntuales++;
                else if (string.Equals(item.Estado, "Tardanza", StringComparison.OrdinalIgnoreCase)) summary.Tardanzas++;
                else if (string.Equals(item.Estado, "Falta", StringComparison.OrdinalIgnoreCase)) summary.Faltas++;
                else if (string.Equals(item.Estado, "Permiso", StringComparison.OrdinalIgnoreCase)) summary.Permisos++;

                summary.HorasTrabajadas += item.HorasTrabajadas;
                if (item.HorasTrabajadas > 8) summary.HorasExtra += item.HorasTrabajadas - 8;
            }

            return summary;
        }

        private EmployeeProfileViewModel GetProfileCore()
        {
            int employeeId = employeeScopeService.RequireCurrentEmployeeId();
            Empleado employee = employeeRepository.GetById(employeeId);
            employeeScopeService.DemandCurrentEmployee(employee.IdEmpleado);
            return new EmployeeProfileViewModel
            {
                IdEmpleado = employee.IdEmpleado,
                Codigo = employee.Codigo,
                NombreCompleto = employee.NombreCompleto,
                Cedula = employee.Cedula,
                Cargo = employee.Cargo,
                Departamento = employee.DepartamentoNombre,
                SalarioBase = employee.SalarioBase,
                Estado = employee.Estado,
                FechaIngreso = employee.FechaIngreso
            };
        }

        private static string NormalizeAttendanceStatus(string status)
        {
            string normalized = (status ?? string.Empty).Trim();
            if (normalized.Length == 0 || string.Equals(normalized, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            string[] allowed = { "Puntual", "Tardanza", "Falta", "Permiso" };
            for (int i = 0; i < allowed.Length; i++)
            {
                if (string.Equals(normalized, allowed[i], StringComparison.OrdinalIgnoreCase)) return allowed[i];
            }

            throw new ArgumentException("El estado de asistencia no es valido.");
        }
    }
}
