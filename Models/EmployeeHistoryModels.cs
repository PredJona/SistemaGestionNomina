using System;

namespace SistemaGestionNomina.Models
{
    public static class EmployeeHistoryFields
    {
        public const string BaseSalary = "SalarioBase";
        public const string Position = "Cargo";
        public const string Department = "Departamento";
        public const string Status = "Estado";
    }

    public sealed class HistorialEmpleado
    {
        public int IdHistorial { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string EmpleadoNombre { get; set; }
        public string CampoModificado { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public string ValorAnteriorTecnico { get; set; }
        public string ValorNuevoTecnico { get; set; }
        public DateTime FechaCambio { get; set; }
        public DateTime FechaEfectiva { get; set; }
        public string UsuarioResponsable { get; set; }
        public string Motivo { get; set; }
        public bool Aplicado { get; set; }
        public DateTime? FechaAplicacion { get; set; }
    }

    public sealed class EmployeeChangeContext
    {
        public DateTime EffectiveDate { get; set; }
        public string Reason { get; set; }
    }

    public sealed class EmployeeSaveResult
    {
        public int EmployeeId { get; set; }
        public int HistoryRecords { get; set; }
        public bool AppliedImmediately { get; set; }
        public bool HasScheduledChanges { get; set; }
    }

    public sealed class EmployeeHistoryQuery
    {
        public int? EmployeeId { get; set; }
        public string Field { get; set; }
        public string User { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
