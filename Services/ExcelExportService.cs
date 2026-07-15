using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public class ExcelExportService
    {
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public string ExportarEmpleados(List<Empleado> empleados)
        {
            authorizationService.DemandAny(Permissions.EmployeesExport, Permissions.ReportsPersonal);
            string path = BuildPath("Empleados");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Empleados");
                WriteHeader(sheet, "Código", "Nombre", "Cédula", "Cargo", "Departamento", "Salario Base", "Estado");
                for (int i = 0; i < empleados.Count; i++)
                {
                    Empleado e = empleados[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = e.Codigo;
                    sheet.Cell(row, 2).Value = e.NombreCompleto;
                    sheet.Cell(row, 3).Value = e.Cedula;
                    sheet.Cell(row, 4).Value = e.Cargo;
                    sheet.Cell(row, 5).Value = e.DepartamentoNombre;
                    sheet.Cell(row, 6).Value = e.SalarioBase;
                    sheet.Cell(row, 7).Value = e.Estado;
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }

            auditTrailService.RegistrarAccion("Exportaciones", "Excel empleados", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarAsistencias(List<Asistencia> asistencias)
        {
            authorizationService.DemandPermission(Permissions.AttendanceExport);
            string path = BuildPath("Asistencia");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Asistencia");
                WriteHeader(sheet, "Empleado", "Fecha", "Entrada", "Salida", "Horas", "Estado");
                for (int i = 0; i < asistencias.Count; i++)
                {
                    Asistencia a = asistencias[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = a.EmpleadoNombre;
                    sheet.Cell(row, 2).Value = a.Fecha.ToString("dd/MM/yyyy");
                    sheet.Cell(row, 3).Value = a.HoraEntrada.HasValue ? a.HoraEntrada.Value.ToString(@"hh\:mm") : "--";
                    sheet.Cell(row, 4).Value = a.HoraSalida.HasValue ? a.HoraSalida.Value.ToString(@"hh\:mm") : "--";
                    sheet.Cell(row, 5).Value = a.HorasTrabajadas;
                    sheet.Cell(row, 6).Value = a.Estado;
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }

            auditTrailService.RegistrarAccion("Exportaciones", "Excel asistencia", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarHistorialEmpleados(List<HistorialEmpleado> history)
        {
            authorizationService.DemandPermission(Permissions.EmployeeHistoryExport);
            string path = BuildPath("Historial_Laboral");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Historial laboral");
                WriteHeader(sheet, "Código", "Empleado", "Campo", "Valor anterior", "Valor nuevo",
                    "Fecha efectiva", "Registrado", "Usuario", "Motivo", "Aplicado");
                for (int i = 0; i < history.Count; i++)
                {
                    HistorialEmpleado item = history[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = item.CodigoEmpleado;
                    sheet.Cell(row, 2).Value = item.EmpleadoNombre;
                    sheet.Cell(row, 3).Value = item.CampoModificado;
                    sheet.Cell(row, 4).Value = item.ValorAnterior;
                    sheet.Cell(row, 5).Value = item.ValorNuevo;
                    sheet.Cell(row, 6).Value = item.FechaEfectiva.ToString("dd/MM/yyyy");
                    sheet.Cell(row, 7).Value = item.FechaCambio.ToString("dd/MM/yyyy HH:mm");
                    sheet.Cell(row, 8).Value = item.UsuarioResponsable;
                    sheet.Cell(row, 9).Value = item.Motivo;
                    sheet.Cell(row, 10).Value = item.Aplicado ? "Sí" : "Programado";
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }
            auditTrailService.RegistrarAccion("Exportaciones", "Excel historial laboral",
                System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarAusencias(List<SolicitudAusencia> requests)
        {
            authorizationService.DemandPermission(Permissions.AbsencesExport);
            string path = BuildPath("Solicitudes_Ausencia");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Ausencias");
                WriteHeader(sheet, "Código", "Empleado", "Departamento", "Tipo", "Desde", "Hasta",
                    "Estado", "Solicitante", "Resolución", "Motivo");
                for (int i = 0; i < requests.Count; i++)
                {
                    SolicitudAusencia item = requests[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = item.CodigoEmpleado;
                    sheet.Cell(row, 2).Value = item.EmpleadoNombre;
                    sheet.Cell(row, 3).Value = item.DepartamentoNombre;
                    sheet.Cell(row, 4).Value = item.Tipo;
                    sheet.Cell(row, 5).Value = item.FechaInicio.ToString("dd/MM/yyyy");
                    sheet.Cell(row, 6).Value = item.FechaFin.ToString("dd/MM/yyyy");
                    sheet.Cell(row, 7).Value = item.Estado;
                    sheet.Cell(row, 8).Value = item.UsuarioSolicitante;
                    sheet.Cell(row, 9).Value = item.ObservacionResolucion;
                    sheet.Cell(row, 10).Value = item.Motivo;
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }
            auditTrailService.RegistrarAccion("Exportaciones", "Excel ausencias",
                System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarNomina(Nomina nomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollExport);
            string path = BuildPath("Nomina");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Nómina");
                sheet.Cell(1, 1).Value = nomina.PeriodoNombre;
                sheet.Range(1, 1, 1, 9).Merge().Style.Font.Bold = true;
                WriteHeader(sheet, 3, "Código", "Empleado", "Departamento", "Sueldo Base", "Bonos", "Horas Extra", "Monto H.E.", "Deducciones", "Neto");
                for (int i = 0; i < nomina.Detalles.Count; i++)
                {
                    NominaDetalle d = nomina.Detalles[i];
                    int row = i + 4;
                    sheet.Cell(row, 1).Value = d.CodigoEmpleado;
                    sheet.Cell(row, 2).Value = d.EmpleadoNombre;
                    sheet.Cell(row, 3).Value = d.Departamento;
                    sheet.Cell(row, 4).Value = d.SueldoBase;
                    sheet.Cell(row, 5).Value = d.Bonos;
                    sheet.Cell(row, 6).Value = d.HorasExtra;
                    sheet.Cell(row, 7).Value = d.MontoHorasExtra;
                    sheet.Cell(row, 8).Value = d.TotalDeducciones;
                    sheet.Cell(row, 9).Value = d.NetoPagar;
                }
                int totalRow = nomina.Detalles.Count + 5;
                sheet.Cell(totalRow, 7).Value = "Totales";
                sheet.Cell(totalRow, 8).Value = nomina.TotalDeducciones;
                sheet.Cell(totalRow, 9).Value = nomina.TotalNeto;
                Finish(sheet);
                workbook.SaveAs(path);
            }

            auditTrailService.RegistrarAccion("Exportaciones", "Excel nómina", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarComprobantes(List<Comprobante> comprobantes)
        {
            authorizationService.DemandPermission(Permissions.PayslipsExport);
            string path = BuildPath("Comprobantes");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Comprobantes");
                WriteHeader(sheet, "Número", "Empleado", "Periodo", "Fecha", "Ingresos", "Deducciones", "Neto");
                for (int i = 0; i < comprobantes.Count; i++)
                {
                    Comprobante c = comprobantes[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = c.NumeroComprobante;
                    sheet.Cell(row, 2).Value = c.EmpleadoNombre;
                    sheet.Cell(row, 3).Value = c.PeriodoNombre;
                    sheet.Cell(row, 4).Value = c.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");
                    sheet.Cell(row, 5).Value = c.TotalIngresos;
                    sheet.Cell(row, 6).Value = c.TotalDeducciones;
                    sheet.Cell(row, 7).Value = c.NetoPagar;
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }

            auditTrailService.RegistrarAccion("Exportaciones", "Excel comprobantes", System.IO.Path.GetFileName(path));
            return path;
        }

        public string ExportarReportes(List<ReporteGenerado> reportes)
        {
            authorizationService.DemandPermission(Permissions.ReportsExport);
            string path = BuildPath("Reportes");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.Worksheets.Add("Reportes");
                WriteHeader(sheet, "Nombre", "Tipo", "Generado por", "Fecha", "Ruta");
                for (int i = 0; i < reportes.Count; i++)
                {
                    ReporteGenerado r = reportes[i];
                    int row = i + 2;
                    sheet.Cell(row, 1).Value = r.NombreReporte;
                    sheet.Cell(row, 2).Value = r.Tipo;
                    sheet.Cell(row, 3).Value = r.GeneradoPor;
                    sheet.Cell(row, 4).Value = r.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");
                    sheet.Cell(row, 5).Value = r.RutaArchivo;
                }
                Finish(sheet);
                workbook.SaveAs(path);
            }

            auditTrailService.RegistrarAccion("Exportaciones", "Excel reportes", System.IO.Path.GetFileName(path));
            return path;
        }

        private static string BuildPath(string prefix)
        {
            // El usuario elige la ubicacion final; la aplicacion no guarda exportaciones en rutas fijas.
            return PathHelper.RequestExportPath(prefix, ".xlsx", "Libro de Excel (*.xlsx)|*.xlsx");
        }

        private static void WriteHeader(IXLWorksheet sheet, params string[] headers)
        {
            WriteHeader(sheet, 1, headers);
        }

        private static void WriteHeader(IXLWorksheet sheet, int row, params string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                sheet.Cell(row, i + 1).Value = headers[i];
                sheet.Cell(row, i + 1).Style.Font.Bold = true;
                sheet.Cell(row, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#A78BFA");
            }
        }

        private static void Finish(IXLWorksheet sheet)
        {
            sheet.Columns().AdjustToContents();
        }
    }
}
