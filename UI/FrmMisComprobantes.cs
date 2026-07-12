using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmMisComprobantes : Form
    {
        private readonly EmployeePortalService portalService = new EmployeePortalService();
        private readonly PrintDocument printDocument = new PrintDocument();
        private List<Comprobante> currentItems = new List<Comprobante>();
        private Comprobante selectedPayslip;

        public FrmMisComprobantes()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void FrmMisComprobantes_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) LoadPayslips();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) LoadPayslips();
        }

        private void dgvComprobantes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComprobantes.CurrentRow == null || dgvComprobantes.CurrentRow.Cells["colId"].Value == null) return;
            try
            {
                selectedPayslip = portalService.GetMyPayslipById(
                    Convert.ToInt32(dgvComprobantes.CurrentRow.Cells["colId"].Value));
                RenderPreview();
            }
            catch (Exception ex)
            {
                selectedPayslip = null;
                lblVistaPrevia.Text = ex.Message;
            }
        }

        private void LoadPayslips()
        {
            try
            {
                currentItems = portalService.GetMyPayslips(txtBuscar.Text);
                dgvComprobantes.Rows.Clear();
                foreach (Comprobante item in currentItems)
                {
                    dgvComprobantes.Rows.Add(item.IdComprobante, item.NumeroComprobante,
                        item.PeriodoNombre, item.FechaGeneracion.ToString("dd/MM/yyyy"),
                        item.TotalIngresos.ToString("0.00"), item.TotalDeducciones.ToString("0.00"),
                        item.NetoPagar.ToString("0.00"));
                }

                if (currentItems.Count == 0)
                {
                    selectedPayslip = null;
                    lblVistaPrevia.Text = "No hay comprobantes disponibles.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mis comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RenderPreview()
        {
            if (selectedPayslip == null) return;
            lblVistaPrevia.Text = "COMPROBANTE DE PAGO\n\n" +
                "Numero: " + selectedPayslip.NumeroComprobante + "\n" +
                "Periodo: " + selectedPayslip.PeriodoNombre + "\n" +
                "Fecha: " + selectedPayslip.FechaGeneracion.ToString("dd/MM/yyyy") + "\n\n" +
                "Ingresos: B/. " + selectedPayslip.TotalIngresos.ToString("#,##0.00") + "\n" +
                "Deducciones: B/. " + selectedPayslip.TotalDeducciones.ToString("#,##0.00") + "\n\n" +
                "NETO A PAGAR: B/. " + selectedPayslip.NetoPagar.ToString("#,##0.00");
        }

        private bool EnsureSelected()
        {
            if (selectedPayslip != null) return true;
            MessageBox.Show("Seleccione un comprobante.", "Mis comprobantes",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            try
            {
                string path = portalService.DownloadMyPayslipPdf(selectedPayslip.IdComprobante);
                if (string.IsNullOrWhiteSpace(path)) return;
                selectedPayslip.RutaPdf = path;
                UiFactory.ShowExported(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Descargar PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirPdf_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            try
            {
                string path = portalService.GetExistingMyPayslipPdf(selectedPayslip.IdComprobante);
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Abrir PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            try
            {
                selectedPayslip = portalService.GetMyPayslipForPrint(selectedPayslip.IdComprobante);
                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = printDocument;
                    preview.Width = 900;
                    preview.Height = 700;
                    preview.StartPosition = FormStartPosition.CenterParent;
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Imprimir comprobante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (selectedPayslip == null)
            {
                e.HasMorePages = false;
                return;
            }

            ComprobantePrintRenderer.Draw(e.Graphics, e.MarginBounds, selectedPayslip);
            e.HasMorePages = false;
        }
    }
}
