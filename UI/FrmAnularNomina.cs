using System;
using System.Windows.Forms;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.UI
{
    public partial class FrmAnularNomina : Form
    {
        private TextBox txtMotivo;
        private Label lblAdvertencia;
        private Label lblResumen;
        private Button btnConfirmar;
        private Button btnCancelar;

        public string Motivo { get; private set; }

        public FrmAnularNomina(Nomina nomina)
        {
            InitializeComponent();
            lblResumen.Text = "Nómina: " + (nomina.PeriodoNombre ?? "Sin nombre") +
                "\nEstado actual: " + (nomina.Estado ?? "Desconocido") +
                "\nTotal neto: B/. " + nomina.TotalNeto.ToString("0.00") +
                "\nEmpleados: " + nomina.Detalles.Count;
        }

        private void InitializeComponent()
        {
            this.lblResumen = new Label();
            this.lblAdvertencia = new Label();
            this.txtMotivo = new TextBox();
            this.btnConfirmar = new Button();
            this.btnCancelar = new Button();
            this.SuspendLayout();

            this.lblResumen.AutoSize = true;
            this.lblResumen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblResumen.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblResumen.Location = new System.Drawing.Point(16, 16);
            this.lblResumen.Size = new System.Drawing.Size(50, 19);
            this.lblResumen.Text = "Resumen";

            this.lblAdvertencia.AutoSize = true;
            this.lblAdvertencia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAdvertencia.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.lblAdvertencia.Location = new System.Drawing.Point(16, 96);
            this.lblAdvertencia.Size = new System.Drawing.Size(80, 15);
            this.lblAdvertencia.Text = "La nómina anterior seguirá existiendo como registro histórico.";

            this.txtMotivo.Location = new System.Drawing.Point(16, 124);
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Size = new System.Drawing.Size(400, 80);
            this.txtMotivo.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtMotivo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtMotivo.BorderStyle = BorderStyle.FixedSingle;

            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnConfirmar.FlatStyle = FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(220, 220);
            this.btnConfirmar.Size = new System.Drawing.Size(100, 36);
            this.btnConfirmar.Text = "Anular";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new EventHandler(this.btnConfirmar_Click);

            this.btnCancelar.BackColor = System.Drawing.Color.Black;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnCancelar.Location = new System.Drawing.Point(336, 220);
            this.btnCancelar.Size = new System.Drawing.Size(80, 36);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(434, 271);
            this.Controls.Add(this.lblResumen);
            this.Controls.Add(this.lblAdvertencia);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.btnCancelar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Anular nómina";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Debe indicar el motivo de la anulación.", "Anular",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Motivo = txtMotivo.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
