using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmReportes : Form
    {
        private readonly ReporteService reporteService = new ReporteService();
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();

        public FrmReportes()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadReports();
            }
        }

        private void btnReporteNomina_Click(object sender, EventArgs e)
        {
            GenerateReport("Reporte general de nómina", "General", "PDF");
        }

        private void btnReporteEmpleados_Click(object sender, EventArgs e)
        {
            GenerateReport("Empleados activos", "Personal", "PDF");
        }

        private void btnReportePagos_Click(object sender, EventArgs e)
        {
            GenerateReport("Historial de pagos", "Histórico", "PDF");
        }

        private void btnReporteDeducciones_Click(object sender, EventArgs e)
        {
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
                    path = excelExportService.ExportarEmpleados(empleadoService.GetActive(null));
                }
                else if (name == "Empleados activos")
                {
                    path = pdfExportService.ExportarEmpleados(empleadoService.GetActive(null));
                }
                else if (format == "Excel")
                {
                    path = excelExportService.ExportarReportes(reporteService.GetAll());
                }
                else
                {
                    path = pdfExportService.ExportarReportes(reporteService.GetAll());
                }

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
