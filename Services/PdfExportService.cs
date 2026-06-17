using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Services
{
    public class PdfExportService
    {
        public string ExportarEmpleados(List<Empleado> empleados)
        {
            string path = BuildPath("Exports\\PDF", "Empleados");
            PdfDocument document = CreateDocument("Empleados");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, "Reporte de Empleados");
            int y = 80;
            for (int i = 0; i < empleados.Count; i++)
            {
                Empleado e = empleados[i];
                DrawLine(gfx, 40, y, e.Codigo + " | " + e.NombreCompleto + " | " + e.DepartamentoNombre + " | B/. " + e.SalarioBase.ToString("0.00") + " | " + e.Estado);
                y = NextLine(document, ref page, ref gfx, y);
            }
            document.Save(path);
            return path;
        }

        public string ExportarAsistencias(List<Asistencia> asistencias)
        {
            string path = BuildPath("Exports\\PDF", "Asistencia");
            PdfDocument document = CreateDocument("Asistencia");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, "Reporte de Asistencia");
            int y = 80;
            for (int i = 0; i < asistencias.Count; i++)
            {
                Asistencia a = asistencias[i];
                DrawLine(gfx, 40, y, a.Fecha.ToString("dd/MM/yyyy") + " | " + a.EmpleadoNombre + " | " + a.HorasTrabajadas.ToString("0.00") + " h | " + a.Estado);
                y = NextLine(document, ref page, ref gfx, y);
            }
            document.Save(path);
            return path;
        }

        public string ExportarNomina(Nomina nomina)
        {
            string path = BuildPath("Exports\\PDF", "Nomina");
            PdfDocument document = CreateDocument("Nómina");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, nomina.PeriodoNombre);
            int y = 80;
            for (int i = 0; i < nomina.Detalles.Count; i++)
            {
                NominaDetalle d = nomina.Detalles[i];
                DrawLine(gfx, 40, y, d.CodigoEmpleado + " | " + d.EmpleadoNombre + " | Ingresos B/. " + d.TotalIngresos.ToString("0.00") + " | Deducciones B/. " + d.TotalDeducciones.ToString("0.00") + " | Neto B/. " + d.NetoPagar.ToString("0.00"));
                y = NextLine(document, ref page, ref gfx, y);
            }
            y += 20;
            DrawLine(gfx, 40, y, "Total neto: B/. " + nomina.TotalNeto.ToString("0.00"));
            document.Save(path);
            return path;
        }

        public string ExportarComprobante(Comprobante comprobante)
        {
            string path = Path.Combine(PathHelper.EnsureExportFolder("Exports\\Comprobantes"),
                comprobante.NumeroComprobante + "_" + PathHelper.Timestamp() + ".pdf");
            PdfDocument document = CreateDocument("Comprobante");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, "Sistema de Gestión de Nómina");
            DrawLine(gfx, 40, 82, "COMPROBANTE DE PAGO");
            DrawLine(gfx, 40, 120, "Número: " + comprobante.NumeroComprobante);
            DrawLine(gfx, 40, 145, "Empleado: " + comprobante.EmpleadoNombre + " (" + comprobante.CodigoEmpleado + ")");
            DrawLine(gfx, 40, 170, "Periodo: " + comprobante.PeriodoNombre);
            DrawLine(gfx, 40, 220, "Total ingresos: B/. " + comprobante.TotalIngresos.ToString("0.00"));
            DrawLine(gfx, 40, 245, "Total deducciones: B/. " + comprobante.TotalDeducciones.ToString("0.00"));
            DrawLine(gfx, 40, 280, "Neto a pagar: B/. " + comprobante.NetoPagar.ToString("0.00"));
            DrawLine(gfx, 40, 340, "Documento generado para fines académicos.");
            document.Save(path);
            return path;
        }

        public string ExportarReportes(List<ReporteGenerado> reportes)
        {
            string path = BuildPath("Exports\\PDF", "Reportes");
            PdfDocument document = CreateDocument("Reportes");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, "Reportes Generados");
            int y = 80;
            for (int i = 0; i < reportes.Count; i++)
            {
                ReporteGenerado r = reportes[i];
                DrawLine(gfx, 40, y, r.NombreReporte + " | " + r.Tipo + " | " + r.FechaGeneracion.ToString("dd/MM/yyyy HH:mm"));
                y = NextLine(document, ref page, ref gfx, y);
            }
            document.Save(path);
            return path;
        }

        private static PdfDocument CreateDocument(string title)
        {
            PdfFontResolver.EnsureRegistered();
            PdfDocument document = new PdfDocument();
            document.Info.Title = title;
            return document;
        }

        private static string BuildPath(string folder, string prefix)
        {
            return Path.Combine(PathHelper.EnsureExportFolder(folder), prefix + "_" + PathHelper.Timestamp() + ".pdf");
        }

        private static void DrawTitle(XGraphics gfx, string title)
        {
            XFont font = new XFont("Segoe UI", 18, XFontStyleEx.Bold);
            gfx.DrawString(title, font, XBrushes.Black, new XRect(40, 35, 520, 30), XStringFormats.TopLeft);
        }

        private static void DrawLine(XGraphics gfx, int x, int y, string text)
        {
            XFont font = new XFont("Segoe UI", 10, XFontStyleEx.Regular);
            gfx.DrawString(text, font, XBrushes.Black, new XRect(x, y, 520, 18), XStringFormats.TopLeft);
        }

        private static int NextLine(PdfDocument document, ref PdfPage page, ref XGraphics gfx, int currentY)
        {
            int next = currentY + 22;
            if (next > 760)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                return 45;
            }

            return next;
        }
    }
}
