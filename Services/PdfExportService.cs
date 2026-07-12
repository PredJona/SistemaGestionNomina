using System;
using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public class PdfExportService
    {
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();

        public string ExportarEmpleados(List<Empleado> empleados)
        {
            authorizationService.DemandAny(Permissions.EmployeesExport, Permissions.ReportsPersonal);
            string path = BuildPath("Empleados");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

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
            auditTrailService.RegistrarAccion("Exportaciones", "PDF empleados", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarAsistencias(List<Asistencia> asistencias)
        {
            authorizationService.DemandPermission(Permissions.AttendanceExport);
            string path = BuildPath("Asistencia");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

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
            auditTrailService.RegistrarAccion("Exportaciones", "PDF asistencia", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarNomina(Nomina nomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollExport);
            string path = BuildPath("Nomina");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

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
            auditTrailService.RegistrarAccion("Exportaciones", "PDF nómina", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarComprobante(Comprobante comprobante)
        {
            authorizationService.DemandPermission(Permissions.PayslipsExport);
            string path = PathHelper.RequestExportPath(comprobante.NumeroComprobante, ".pdf", "Documento PDF (*.pdf)|*.pdf");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            SaveComprobantePdf(comprobante, path);
            auditTrailService.RegistrarAccion("Exportaciones", "PDF comprobante", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarComprobantePersonal(Comprobante comprobante)
        {
            authorizationService.DemandPermission(Permissions.OwnPayslipsDownload);
            if (comprobante == null) throw new ArgumentNullException("comprobante");
            employeeScopeService.DemandCurrentEmployee(comprobante.IdEmpleado);
            string path = PathHelper.RequestExportPath(comprobante.NumeroComprobante, ".pdf", "Documento PDF (*.pdf)|*.pdf");
            return ExportarComprobantePersonal(comprobante, path);
        }

        internal string ExportarComprobantePersonal(Comprobante comprobante, string path)
        {
            authorizationService.DemandPermission(Permissions.OwnPayslipsDownload);
            if (comprobante == null) throw new ArgumentNullException("comprobante");
            employeeScopeService.DemandCurrentEmployee(comprobante.IdEmpleado);
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            if (!string.Equals(System.IO.Path.GetExtension(path), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("La ubicacion seleccionada debe ser un archivo PDF.");
            }

            string fullPath = System.IO.Path.GetFullPath(path);
            SaveComprobantePdf(comprobante, fullPath);
            return fullPath;
        }

        public string ExportarReportes(List<ReporteGenerado> reportes)
        {
            authorizationService.DemandPermission(Permissions.ReportsExport);
            string path = BuildPath("Reportes");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

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
            auditTrailService.RegistrarAccion("Exportaciones", "PDF reportes", System.IO.Path.GetFileName(path));
            return path;
        }

        private static PdfDocument CreateDocument(string title)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = title;
            return document;
        }

        private static void SaveComprobantePdf(Comprobante comprobante, string path)
        {
            PdfDocument document = CreateDocument("Comprobante");
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawTitle(gfx, "Proy2_Eq01_CamposPD");
            DrawLine(gfx, 40, 82, "COMPROBANTE DE PAGO");
            DrawLine(gfx, 40, 120, "Numero: " + comprobante.NumeroComprobante);
            DrawLine(gfx, 40, 145, "Empleado: " + comprobante.EmpleadoNombre + " (" + comprobante.CodigoEmpleado + ")");
            DrawLine(gfx, 40, 170, "Periodo: " + comprobante.PeriodoNombre);
            DrawLine(gfx, 40, 220, "Total ingresos: B/. " + comprobante.TotalIngresos.ToString("0.00"));
            DrawLine(gfx, 40, 245, "Total deducciones: B/. " + comprobante.TotalDeducciones.ToString("0.00"));
            DrawLine(gfx, 40, 280, "Neto a pagar: B/. " + comprobante.NetoPagar.ToString("0.00"));
            DrawLine(gfx, 40, 340, "Documento generado para fines academicos.");
            document.Save(path);
        }

        private static string BuildPath(string prefix)
        {
            return PathHelper.RequestExportPath(prefix, ".pdf", "Documento PDF (*.pdf)|*.pdf");
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
