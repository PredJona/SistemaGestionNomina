using System;
using System.Collections.Generic;

namespace SistemaGestionNomina.Models
{
    public static class PayrollStates
    {
        public const string Draft = "Borrador";
        public const string Calculated = "Calculada";
        public const string Confirmed = "Confirmada";
        public const string Paid = "Pagada";
        public const string Annulled = "Anulada";

        public static bool IsValid(string state)
        {
            return string.Equals(state, Draft, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(state, Calculated, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(state, Confirmed, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(state, Paid, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(state, Annulled, StringComparison.OrdinalIgnoreCase);
        }
    }

    public static class PayrollPeriodStates
    {
        public const string Open = "Abierto";
        public const string Confirmed = "Confirmado";
        public const string Paid = "Pagado";
        public const string Reopened = "Reabierto";
    }

    public static class PayrollStateMachine
    {
        private static readonly IDictionary<string, string[]> AllowedTransitions =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { PayrollStates.Draft, new[] { PayrollStates.Calculated } },
                { PayrollStates.Calculated, new[] { PayrollStates.Confirmed } },
                { PayrollStates.Confirmed, new[] { PayrollStates.Paid, PayrollStates.Annulled } },
                { PayrollStates.Paid, new[] { PayrollStates.Annulled } },
                { PayrollStates.Annulled, new string[0] }
            };

        public static bool CanTransition(string currentState, string targetState)
        {
            if (string.IsNullOrWhiteSpace(currentState) || string.IsNullOrWhiteSpace(targetState))
                return false;

            string[] targets;
            if (!AllowedTransitions.TryGetValue(currentState.Trim(), out targets)) return false;
            for (int i = 0; i < targets.Length; i++)
            {
                if (string.Equals(targets[i], targetState.Trim(), StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public static void DemandTransition(string currentState, string targetState)
        {
            if (!CanTransition(currentState, targetState))
            {
                throw new InvalidOperationException("No se permite cambiar la nómina de " +
                    (currentState ?? "sin estado") + " a " + (targetState ?? "sin estado") + ".");
            }
        }
    }
}
