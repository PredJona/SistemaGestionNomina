using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.UI
{
    public partial class FrmComprobantes : Form
    {
        private readonly ComprobanteService comprobanteService = new ComprobanteService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly EmailService emailService = new EmailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly PrintDocument printDocument = new PrintDocument();
        private Comprobante selectedComprobante;
        private List<Comprobante> currentItems = new List<Comprobante>();

        public FrmComprobantes()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void FrmComprobantes_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                btnExportarExcel.Visible = authorizationService.HasPermission(Permissions.PayslipsExport);
                btnExportarPdf.Visible = authorizationService.HasPermission(Permissions.PayslipsExport);
                btnImprimir.Visible = authorizationService.HasPermission(Permissions.PayslipsPrint);
                btnEnviarEmail.Visible = authorizationService.HasPermission(Permissions.PayslipsEmail);
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
                "------------------------------------------------\n" +
                "Documento:      " + selectedComprobante.NumeroComprobante + "\n" +
                "Fecha emisión:  " + selectedComprobante.FechaGeneracion.ToString("dd/MM/yyyy HH:mm") + "\n" +
                "Periodo:        " + selectedComprobante.PeriodoNombre + "\n" +
                "------------------------------------------------\n" +
                "Empleado:       " + selectedComprobante.EmpleadoNombre + "\n" +
                "Código:         " + selectedComprobante.CodigoEmpleado + "\n" +
                "------------------------------------------------\n" +
                "Ingresos:       B/. " + selectedComprobante.TotalIngresos.ToString("0.00") + "\n" +
                "Deducciones:    B/. " + selectedComprobante.TotalDeducciones.ToString("0.00") + "\n" +
                "------------------------------------------------\n" +
                "NETO A PAGAR:   B/. " + selectedComprobante.NetoPagar.ToString("0.00") + "\n" +
                "------------------------------------------------\n\n" +
                "Firma autorizada: __________________________";
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;
            string path = pdfExportService.ExportarComprobante(selectedComprobante);
            if (string.IsNullOrWhiteSpace(path)) return;

            comprobanteService.SaveRutaPdf(selectedComprobante.IdComprobante, path);
            UiFactory.ShowExported(path);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (currentItems.Count == 0)
            {
                MessageBox.Show("No hay comprobantes para exportar.", "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UiFactory.ShowExported(excelExportService.ExportarComprobantes(currentItems));
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            authorizationService.DemandPermission(Permissions.PayslipsPrint);
            if (!EnsureSelected()) return;

            try
            {
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
                MessageBox.Show("No se pudo abrir la vista previa de impresión.\n\n" + ex.Message,
                    "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (selectedComprobante == null)
            {
                e.HasMorePages = false;
                return;
            }

            ComprobantePrintRenderer.Draw(e.Graphics, e.MarginBounds, selectedComprobante);
            e.HasMorePages = false;
        }

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            if (!EnsureSelected()) return;

            string email = PromptEmail();
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            try
            {
                string pdfPath = selectedComprobante.RutaPdf;
                if (string.IsNullOrWhiteSpace(pdfPath) || !File.Exists(pdfPath))
                {
                    pdfPath = pdfExportService.ExportarComprobante(selectedComprobante);
                    if (string.IsNullOrWhiteSpace(pdfPath)) return;

                    comprobanteService.SaveRutaPdf(selectedComprobante.IdComprobante, pdfPath);
                    selectedComprobante.RutaPdf = pdfPath;
                }

                string emlPath = emailService.CrearBorradorComprobante(email, pdfPath,
                    "Comprobante de pago " + selectedComprobante.NumeroComprobante);
                UiFactory.ShowExported(emlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Enviar email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool EnsureSelected()
        {
            if (selectedComprobante != null) return true;
            MessageBox.Show("Seleccione un comprobante.", "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private string PromptEmail()
        {
            using (Form prompt = new Form())
            using (Label label = new Label())
            using (TextBox input = new TextBox())
            using (Button okButton = new Button())
            using (Button cancelButton = new Button())
            {
                prompt.Text = "Enviar comprobante";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;
                prompt.ClientSize = new Size(420, 140);

                label.Text = "Correo del destinatario:";
                label.Location = new Point(18, 18);
                label.Size = new Size(360, 24);

                input.Location = new Point(18, 48);
                input.Size = new Size(380, 24);

                okButton.Text = "Crear email";
                okButton.DialogResult = DialogResult.OK;
                okButton.Location = new Point(210, 92);
                okButton.Size = new Size(92, 30);

                cancelButton.Text = "Cancelar";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Location = new Point(306, 92);
                cancelButton.Size = new Size(92, 30);

                prompt.Controls.Add(label);
                prompt.Controls.Add(input);
                prompt.Controls.Add(okButton);
                prompt.Controls.Add(cancelButton);
                prompt.AcceptButton = okButton;
                prompt.CancelButton = cancelButton;

                return prompt.ShowDialog(this) == DialogResult.OK ? input.Text.Trim() : string.Empty;
            }
        }
    }
}
