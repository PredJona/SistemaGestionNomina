using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmComprobantes : Form
    {
        private readonly ComprobanteService comprobanteService = new ComprobanteService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private Comprobante selectedComprobante;
        private List<Comprobante> currentItems = new List<Comprobante>();

        public FrmComprobantes()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmComprobantes_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadReceipts();
            }
        }

        private void LoadReceipts()
        {
            currentItems = comprobanteService.GetAll(txtBuscar.Text);
            dgvComprobantes.Rows.Clear();
            foreach (Comprobante c in currentItems)
            {
                dgvComprobantes.Rows.Add(c.IdComprobante, c.NumeroComprobante, c.EmpleadoNombre, c.PeriodoNombre,
                    "B/. " + c.NetoPagar.ToString("0.00"));
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadReceipts();
            }
        }

        private void dgvComprobantes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComprobantes.CurrentRow == null || dgvComprobantes.CurrentRow.Cells["IdComprobante"].Value == null)
            {
                return;
            }

            int id = Convert.ToInt32(dgvComprobantes.CurrentRow.Cells["IdComprobante"].Value);
            selectedComprobante = comprobanteService.GetById(id);
            RenderPreview();
        }

        private void RenderPreview()
        {
            if (selectedComprobante == null) return;
            lblVistaPrevia.Text =
                "SISTEMA DE GESTIÓN DE NÓMINA\n" +
                "COMPROBANTE DE PAGO\n\n" +
                "N° Doc: " + selectedComprobante.NumeroComprobante + "\n" +
                "Fecha emisión: " + selectedComprobante.FechaGeneracion.ToString("dd/MM/yyyy HH:mm") + "\n" +
                "Periodo: " + selectedComprobante.PeriodoNombre + "\n\n" +
                "Empleado: " + selectedComprobante.EmpleadoNombre + "\n" +
                "Código: " + selectedComprobante.CodigoEmpleado + "\n\n" +
                "Ingresos: B/. " + selectedComprobante.TotalIngresos.ToString("0.00") + "\n" +
                "Deducciones: B/. " + selectedComprobante.TotalDeducciones.ToString("0.00") + "\n\n" +
                "Neto a pagar: B/. " + selectedComprobante.NetoPagar.ToString("0.00");
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            string path = pdfExportService.ExportarComprobante(selectedComprobante);
            comprobanteService.SaveRutaPdf(selectedComprobante.IdComprobante, path);
            UiFactory.ShowExported(path);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            UiFactory.ShowExported(excelExportService.ExportarComprobantes(currentItems));
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            PrintDocument document = new PrintDocument();
            document.PrintPage += delegate(object s, PrintPageEventArgs args)
            {
                args.Graphics.DrawString(lblVistaPrevia.Text, new Font("Segoe UI", 11F), Brushes.Black, new RectangleF(60, 60, 680, 800));
            };

            try
            {
                document.Print();
                MessageBox.Show("Comprobante enviado a impresión.", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo imprimir.\n\n" + ex.Message, "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad pendiente para completar por otro integrante.",
                "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool EnsureSelected()
        {
            if (selectedComprobante != null) return true;
            MessageBox.Show("Seleccione un comprobante.", "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }
}
