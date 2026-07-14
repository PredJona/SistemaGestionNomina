using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.UI
{
    public partial class FrmHistorialNomina : Form
    {
        private DataGridView dgvHistorial;
        private Button btnCerrar;

        public FrmHistorialNomina(List<NominaVersion> versiones)
        {
            InitializeComponent();
            CargarHistorial(versiones);
        }

        private void InitializeComponent()
        {
            this.dgvHistorial = new DataGridView();
            this.btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();

            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.BackgroundColor = Color.FromArgb(24, 25, 34);
            this.dgvHistorial.BorderStyle = BorderStyle.None;
            this.dgvHistorial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new Point(16, 16);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.Size = new Size(600, 300);
            this.dgvHistorial.TabIndex = 0;
            this.dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistorial.EnableHeadersVisualStyles = false;

            this.dgvHistorial.DefaultCellStyle.BackColor = Color.FromArgb(13, 14, 20);
            this.dgvHistorial.DefaultCellStyle.ForeColor = Color.FromArgb(229, 228, 245);
            this.dgvHistorial.DefaultCellStyle.SelectionBackColor = Color.FromArgb(167, 139, 250);
            this.dgvHistorial.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvHistorial.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            this.dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 18, 26);
            this.dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(167, 139, 250);
            this.dgvHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dgvHistorial.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(20, 21, 30);

            this.dgvHistorial.GridColor = Color.FromArgb(30, 31, 40);

            this.btnCerrar.BackColor = Color.Black;
            this.btnCerrar.FlatStyle = FlatStyle.Flat;
            this.btnCerrar.ForeColor = Color.FromArgb(229, 228, 245);
            this.btnCerrar.Location = new Point(520, 332);
            this.btnCerrar.Size = new Size(96, 32);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new EventHandler(this.btnCerrar_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(13, 14, 20);
            this.ClientSize = new Size(634, 381);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.btnCerrar);
            this.Font = new Font("Segoe UI", 9F);
            this.ForeColor = Color.FromArgb(229, 228, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Historial de versiones";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
        }

        private void CargarHistorial(List<NominaVersion> versiones)
        {
            dgvHistorial.Columns.Clear();
            dgvHistorial.Columns.Add("Version", "Versión");
            dgvHistorial.Columns.Add("Direccion", "Original → Nueva");
            dgvHistorial.Columns.Add("Motivo", "Motivo");
            dgvHistorial.Columns.Add("Usuario", "Usuario");
            dgvHistorial.Columns.Add("Fecha", "Fecha");

            for (int i = 0; i < versiones.Count; i++)
            {
                NominaVersion v = versiones[i];
                string direccion = v.IdNominaOriginal.ToString() + " → " +
                    (v.IdNominaNueva.HasValue ? v.IdNominaNueva.Value.ToString() : "Pendiente");
                dgvHistorial.Rows.Add(
                    v.IdVersion.ToString(),
                    direccion,
                    v.MotivoCambio,
                    v.UsuarioResponsable,
                    v.FechaCambio.ToString("yyyy-MM-dd HH:mm")
                );
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
