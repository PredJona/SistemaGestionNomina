using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.UI
{
    public partial class FrmReportes : Form
    {
        private readonly ReporteService reporteService = new ReporteService();
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();

        public FrmReportes()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                bool personal = authorizationService.HasPermission(Permissions.ReportsPersonal);
                bool financial = authorizationService.HasPermission(Permissions.ReportsFinancial);
                cardEmpleados.Visible = personal;
                cardNomina.Visible = financial;
                cardPagos.Visible = financial;
                cardDeducciones.Visible = financial;
                btnExportarExcel.Visible = personal && financial;
                btnExportarPdf.Visible = personal && financial;
                LoadReports();
            }
        }

        private void btnReporteNomina_Click(object sender, EventArgs e)
        {
            authorizationService.DemandPermission(Permissions.ReportsFinancial);
            GenerateReport("Reporte general de nómina", "General", "PDF");
        }

        private void btnReporteEmpleados_Click(object sender, EventArgs e)
        {
            authorizationService.DemandPermission(Permissions.ReportsPersonal);
            GenerateReport("Empleados activos", "Personal", "PDF");
        }

        private void btnReportePagos_Click(object sender, EventArgs e)
        {
            authorizationService.DemandPermission(Permissions.ReportsFinancial);
            GenerateReport("Historial de pagos", "Histórico", "PDF");
        }

        private void btnReporteDeducciones_Click(object sender, EventArgs e)
        {
            authorizationService.DemandPermission(Permissions.ReportsFinancial);
            GenerateReport("Resumen de deducciones", "Deducciones", "PDF");
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            GenerateReport("Reportes administrativos", "General", "Excel");
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            GenerateReport("Reportes administrativos", "General", "PDF");
        }

        private void GenerateReport(string name, string type, string format)
        {
            try
            {
                string path;
                if (name == "Empleados activos" && format == "Excel")
                {
                    List<Empleado> empleados = empleadoService.GetActive(null);
                    if (empleados.Count == 0)
                    {
                        MessageBox.Show("No hay empleados activos para generar el reporte.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    path = excelExportService.ExportarEmpleados(empleados);
                }
                else if (name == "Empleados activos")
                {
                    List<Empleado> empleados = empleadoService.GetActive(null);
                    if (empleados.Count == 0)
                    {
                        MessageBox.Show("No hay empleados activos para generar el reporte.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    path = pdfExportService.ExportarEmpleados(empleados);
                }
                else if (format == "Excel")
                {
                    List<ReporteGenerado> reportes = reporteService.GetAll();
                    if (name == "Reportes administrativos" && reportes.Count == 0)
                    {
                        MessageBox.Show("No hay reportes registrados para exportar.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    path = excelExportService.ExportarReportes(reportes);
                }
                else
                {
                    List<ReporteGenerado> reportes = reporteService.GetAll();
                    if (name == "Reportes administrativos" && reportes.Count == 0)
                    {
                        MessageBox.Show("No hay reportes registrados para exportar.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    path = pdfExportService.ExportarReportes(reportes);
                }

                if (string.IsNullOrWhiteSpace(path)) return;

                reporteService.Register(name, type + " / " + format, path);
                UiFactory.ShowExported(path);
                LoadReports();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadReports()
        {
            List<ReporteGenerado> reports = reporteService.GetAll();
            dgvReportesRecientes.Rows.Clear();
            foreach (ReporteGenerado r in reports)
            {
                dgvReportesRecientes.Rows.Add(r.NombreReporte, r.Tipo, r.GeneradoPor,
                    r.FechaGeneracion.ToString("dd/MM/yyyy HH:mm"), r.RutaArchivo);
            }
        }
    }
}
