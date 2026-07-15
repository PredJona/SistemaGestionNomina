using System;
using System.Collections.Generic;

namespace SistemaGestionNomina.Models
{
    public static class AbsenceTypes
    {
        public const string Vacation = "Vacaciones";
        public const string PaidLeave = "Permiso remunerado";
        public const string UnpaidLeave = "Permiso no remunerado";
        public const string MedicalLeave = "Incapacidad médica";
        public const string License = "Licencia";
        public const string JustifiedAbsence = "Ausencia justificada";
        public const string UnjustifiedAbsence = "Ausencia injustificada";
        public const string Suspension = "Suspensión";

        public static readonly string[] All =
        {
            Vacation, PaidLeave, UnpaidLeave, MedicalLeave, License,
            JustifiedAbsence, UnjustifiedAbsence, Suspension
        };

        public static bool IsValid(string value)
        {
            for (int i = 0; i < All.Length; i++)
            {
                if (string.Equals(All[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public static string ToAttendanceStatus(string value)
        {
            if (string.Equals(value, UnpaidLeave, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, UnjustifiedAbsence, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, Suspension, StringComparison.OrdinalIgnoreCase))
                return "Falta";
            return "Permiso";
        }
    }

    public static class AbsenceStates
    {
        public const string Pending = "Pendiente";
        public const string Approved = "Aprobada";
        public const string Rejected = "Rechazada";
        public const string Cancelled = "Cancelada";

        public static readonly string[] All = { Pending, Approved, Rejected, Cancelled };

        public static bool IsValid(string value)
        {
            for (int i = 0; i < All.Length; i++)
            {
                if (string.Equals(All[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
    }

    public static class AbsenceStateMachine
    {
        public static bool CanTransition(string currentState, string targetState)
        {
            if (string.Equals(currentState, AbsenceStates.Pending, StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(targetState, AbsenceStates.Approved, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(targetState, AbsenceStates.Rejected, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(targetState, AbsenceStates.Cancelled, StringComparison.OrdinalIgnoreCase);
            }
            return string.Equals(currentState, AbsenceStates.Approved, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(targetState, AbsenceStates.Cancelled, StringComparison.OrdinalIgnoreCase);
        }

        public static void DemandTransition(string currentState, string targetState)
        {
            if (!CanTransition(currentState, targetState))
                throw new InvalidOperationException("La solicitud no puede cambiar de " + currentState + " a " + targetState + ".");
        }
    }

    public sealed class SolicitudAusencia
    {
        public int IdSolicitud { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string EmpleadoNombre { get; set; }
        public string DepartamentoNombre { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public string AprobadoPor { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string ObservacionResolucion { get; set; }
        public string UsuarioSolicitante { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public sealed class AbsenceQuery
    {
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Search { get; set; }
    }

    public sealed class OwnAbsenceRequest
    {
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
