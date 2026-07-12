using System;
using System.Collections.Generic;

namespace SistemaGestionNomina.Models
{
    public sealed class EmployeeProfileViewModel
    {
        public int IdEmpleado { get; set; }
        public string Codigo { get; set; }
        public string NombreCompleto { get; set; }
        public string Cedula { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioBase { get; set; }
        public string Estado { get; set; }
        public DateTime FechaIngreso { get; set; }
    }

    public sealed class EmployeeAttendanceSummaryViewModel
    {
        public int Puntuales { get; set; }
        public int Tardanzas { get; set; }
        public int Faltas { get; set; }
        public int Permisos { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtra { get; set; }
    }

    public sealed class EmployeePortalDashboardViewModel
    {
        public EmployeeProfileViewModel Profile { get; set; }
        public Comprobante LatestPayslip { get; set; }
        public List<Asistencia> RecentAttendance { get; set; }
        public EmployeeAttendanceSummaryViewModel AttendanceSummary { get; set; }
    }
}
