using System.Drawing;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Helpers
{
    public static class ComprobantePrintRenderer
    {
        public static void Draw(Graphics graphics, Rectangle bounds, Comprobante comprobante)
        {
            int y = bounds.Top;
            using (Font titleFont = new Font("Segoe UI", 18F, FontStyle.Bold))
            using (Font headerFont = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (Font bodyFont = new Font("Segoe UI", 10F))
            using (Font totalFont = new Font("Segoe UI", 13F, FontStyle.Bold))
            using (Pen linePen = new Pen(Color.Black, 1F))
            {
                graphics.DrawString("Proy2_Eq01_CamposPD", titleFont, Brushes.Black, bounds.Left, y);
                y += 42;
                graphics.DrawString("Comprobante de Pago", headerFont, Brushes.Black, bounds.Left, y);
                y += 34;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 22;
                DrawRow(graphics, bodyFont, "Documento:", comprobante.NumeroComprobante, bounds.Left, ref y);
                DrawRow(graphics, bodyFont, "Fecha emision:", comprobante.FechaGeneracion.ToString("dd/MM/yyyy HH:mm"), bounds.Left, ref y);
                DrawRow(graphics, bodyFont, "Periodo:", comprobante.PeriodoNombre, bounds.Left, ref y);
                y += 8;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 22;
                DrawRow(graphics, bodyFont, "Empleado:", comprobante.EmpleadoNombre, bounds.Left, ref y);
                DrawRow(graphics, bodyFont, "Codigo:", comprobante.CodigoEmpleado, bounds.Left, ref y);
                y += 8;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 22;
                DrawRow(graphics, bodyFont, "Ingresos:", "B/. " + comprobante.TotalIngresos.ToString("0.00"), bounds.Left, ref y);
                DrawRow(graphics, bodyFont, "Deducciones:", "B/. " + comprobante.TotalDeducciones.ToString("0.00"), bounds.Left, ref y);
                y += 12;
                graphics.DrawString("NETO A PAGAR: B/. " + comprobante.NetoPagar.ToString("0.00"), totalFont, Brushes.Black, bounds.Left, y);
                y += 54;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Left + 300, y);
                graphics.DrawString("Firma autorizada", bodyFont, Brushes.Black, bounds.Left, y + 8);
            }
        }

        private static void DrawRow(Graphics graphics, Font font, string label, string value, int x, ref int y)
        {
            graphics.DrawString(label, font, Brushes.Black, x, y);
            graphics.DrawString(value ?? string.Empty, font, Brushes.Black, x + 140, y);
            y += 26;
        }
    }
}
