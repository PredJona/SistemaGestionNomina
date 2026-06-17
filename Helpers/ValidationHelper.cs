using System;
using System.Windows.Forms;

namespace SistemaGestionNomina.Helpers
{
    public static class ValidationHelper
    {
        public static bool RequireText(TextBox textBox, string fieldName)
        {
            if (textBox == null || !string.IsNullOrWhiteSpace(textBox.Text))
            {
                return true;
            }

            MessageBox.Show(fieldName + " es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (textBox != null)
            {
                textBox.Focus();
            }

            return false;
        }

        public static bool TryPositiveDecimal(TextBox textBox, string fieldName, out decimal value)
        {
            value = 0;
            if (textBox != null && decimal.TryParse(textBox.Text, out value) && value > 0)
            {
                return true;
            }

            MessageBox.Show(fieldName + " debe ser mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (textBox != null)
            {
                textBox.Focus();
            }

            return false;
        }

        public static bool ValidateDateRange(DateTime start, DateTime end)
        {
            if (start.Date <= end.Date)
            {
                return true;
            }

            MessageBox.Show("La fecha de inicio debe ser menor o igual que la fecha fin.",
                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        public static bool ValidateHourRange(TimeSpan entry, TimeSpan exit)
        {
            if (exit > entry)
            {
                return true;
            }

            MessageBox.Show("La hora de salida debe ser mayor que la hora de entrada.",
                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}
