namespace SistemaGestionNomina.UI
{
    partial class FrmGestionAusencias
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo, lblSolicitud, lblDetalle, lblMotivo, lblObservacion;
        private System.Windows.Forms.TextBox txtMotivo, txtObservacion;
        private System.Windows.Forms.Button btnAprobar, btnRechazar, btnCancelarSolicitud, btnCerrar;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label(); this.lblSolicitud = new System.Windows.Forms.Label(); this.lblDetalle = new System.Windows.Forms.Label(); this.lblMotivo = new System.Windows.Forms.Label(); this.lblObservacion = new System.Windows.Forms.Label(); this.txtMotivo = new System.Windows.Forms.TextBox(); this.txtObservacion = new System.Windows.Forms.TextBox(); this.btnAprobar = new System.Windows.Forms.Button(); this.btnRechazar = new System.Windows.Forms.Button(); this.btnCancelarSolicitud = new System.Windows.Forms.Button(); this.btnCerrar = new System.Windows.Forms.Button(); this.SuspendLayout();
            this.BackColor = System.Drawing.Color.FromArgb(18, 18, 27); this.ClientSize = new System.Drawing.Size(660, 460); this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog; this.MaximizeBox = false; this.MinimizeBox = false; this.Name = "FrmGestionAusencias"; this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent; this.Text = "Gestión de ausencia"; this.Load += new System.EventHandler(this.FrmGestionAusencias_Load);
            this.lblTitulo.AutoSize = true; this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold); this.lblTitulo.Location = new System.Drawing.Point(28, 20); this.lblTitulo.Text = "Resolver solicitud";
            this.lblSolicitud.AutoSize = true; this.lblSolicitud.ForeColor = System.Drawing.Color.FromArgb(167, 139, 250); this.lblSolicitud.Location = new System.Drawing.Point(32, 72); this.lblSolicitud.Text = "Empleado";
            this.lblDetalle.AutoSize = true; this.lblDetalle.Location = new System.Drawing.Point(32, 99); this.lblDetalle.Text = "Detalle de la solicitud";
            this.lblMotivo.AutoSize = true; this.lblMotivo.Location = new System.Drawing.Point(32, 136); this.lblMotivo.Text = "Motivo del trabajador"; this.txtMotivo.Location = new System.Drawing.Point(35, 158); this.txtMotivo.Multiline = true; this.txtMotivo.ReadOnly = true; this.txtMotivo.Size = new System.Drawing.Size(590, 72);
            this.lblObservacion.AutoSize = true; this.lblObservacion.Location = new System.Drawing.Point(32, 250); this.lblObservacion.Text = "Observación o motivo de resolución"; this.txtObservacion.Location = new System.Drawing.Point(35, 272); this.txtObservacion.MaxLength = 500; this.txtObservacion.Multiline = true; this.txtObservacion.Size = new System.Drawing.Size(590, 72);
            this.btnAprobar.BackColor = System.Drawing.Color.FromArgb(34, 197, 94); this.btnAprobar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnAprobar.Location = new System.Drawing.Point(194, 380); this.btnAprobar.Size = new System.Drawing.Size(94, 34); this.btnAprobar.Text = "Aprobar"; this.btnAprobar.UseVisualStyleBackColor = false; this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            this.btnRechazar.BackColor = System.Drawing.Color.FromArgb(239, 68, 68); this.btnRechazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnRechazar.Location = new System.Drawing.Point(296, 380); this.btnRechazar.Size = new System.Drawing.Size(94, 34); this.btnRechazar.Text = "Rechazar"; this.btnRechazar.UseVisualStyleBackColor = false; this.btnRechazar.Click += new System.EventHandler(this.btnRechazar_Click);
            this.btnCancelarSolicitud.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnCancelarSolicitud.Location = new System.Drawing.Point(398, 380); this.btnCancelarSolicitud.Size = new System.Drawing.Size(108, 34); this.btnCancelarSolicitud.Text = "Cancelar solicitud"; this.btnCancelarSolicitud.Click += new System.EventHandler(this.btnCancelarSolicitud_Click);
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnCerrar.Location = new System.Drawing.Point(514, 380); this.btnCerrar.Size = new System.Drawing.Size(110, 34); this.btnCerrar.Text = "Cerrar"; this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            this.Controls.Add(this.lblTitulo); this.Controls.Add(this.lblSolicitud); this.Controls.Add(this.lblDetalle); this.Controls.Add(this.lblMotivo); this.Controls.Add(this.txtMotivo); this.Controls.Add(this.lblObservacion); this.Controls.Add(this.txtObservacion); this.Controls.Add(this.btnAprobar); this.Controls.Add(this.btnRechazar); this.Controls.Add(this.btnCancelarSolicitud); this.Controls.Add(this.btnCerrar); this.ResumeLayout(false); this.PerformLayout();
        }
    }
}
