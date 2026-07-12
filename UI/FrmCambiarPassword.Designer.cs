namespace SistemaGestionNomina.UI
{
    partial class FrmCambiarPassword
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Panel panelCuenta;
        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.TextBox txtActual;
        private System.Windows.Forms.Label lblNueva;
        private System.Windows.Forms.TextBox txtNueva;
        private System.Windows.Forms.Label lblConfirmar;
        private System.Windows.Forms.TextBox txtConfirmar;
        private System.Windows.Forms.Label lblPolitica;
        private System.Windows.Forms.Button btnGuardar;

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label(); this.lblSubtitulo = new System.Windows.Forms.Label(); this.panelCuenta = new System.Windows.Forms.Panel(); this.lblActual = new System.Windows.Forms.Label(); this.txtActual = new System.Windows.Forms.TextBox(); this.lblNueva = new System.Windows.Forms.Label(); this.txtNueva = new System.Windows.Forms.TextBox(); this.lblConfirmar = new System.Windows.Forms.Label(); this.txtConfirmar = new System.Windows.Forms.TextBox(); this.lblPolitica = new System.Windows.Forms.Label(); this.btnGuardar = new System.Windows.Forms.Button(); this.panelCuenta.SuspendLayout(); this.SuspendLayout();
            this.lblTitulo.AutoSize = true; this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold); this.lblTitulo.Location = new System.Drawing.Point(30, 24); this.lblTitulo.Text = "Cambiar contrasena";
            this.lblSubtitulo.AutoSize = true; this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblSubtitulo.Location = new System.Drawing.Point(34, 78); this.lblSubtitulo.Text = "Actualiza la clave utilizada para iniciar sesion.";
            this.panelCuenta.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.panelCuenta.Controls.Add(this.lblActual); this.panelCuenta.Controls.Add(this.txtActual); this.panelCuenta.Controls.Add(this.lblNueva); this.panelCuenta.Controls.Add(this.txtNueva); this.panelCuenta.Controls.Add(this.lblConfirmar); this.panelCuenta.Controls.Add(this.txtConfirmar); this.panelCuenta.Controls.Add(this.lblPolitica); this.panelCuenta.Controls.Add(this.btnGuardar); this.panelCuenta.Location = new System.Drawing.Point(38, 126); this.panelCuenta.Name = "panelCuenta"; this.panelCuenta.Size = new System.Drawing.Size(560, 444);
            this.lblActual.AutoSize = true; this.lblActual.Location = new System.Drawing.Point(28, 28); this.lblActual.Text = "Contrasena actual";
            this.txtActual.Location = new System.Drawing.Point(28, 56); this.txtActual.Name = "txtActual"; this.txtActual.Size = new System.Drawing.Size(500, 27); this.txtActual.UseSystemPasswordChar = true;
            this.lblNueva.AutoSize = true; this.lblNueva.Location = new System.Drawing.Point(28, 110); this.lblNueva.Text = "Nueva contrasena";
            this.txtNueva.Location = new System.Drawing.Point(28, 138); this.txtNueva.Name = "txtNueva"; this.txtNueva.Size = new System.Drawing.Size(500, 27); this.txtNueva.UseSystemPasswordChar = true;
            this.lblConfirmar.AutoSize = true; this.lblConfirmar.Location = new System.Drawing.Point(28, 192); this.lblConfirmar.Text = "Confirmar nueva contrasena";
            this.txtConfirmar.Location = new System.Drawing.Point(28, 220); this.txtConfirmar.Name = "txtConfirmar"; this.txtConfirmar.Size = new System.Drawing.Size(500, 27); this.txtConfirmar.UseSystemPasswordChar = true;
            this.lblPolitica.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblPolitica.Location = new System.Drawing.Point(28, 274); this.lblPolitica.Name = "lblPolitica"; this.lblPolitica.Size = new System.Drawing.Size(500, 52); this.lblPolitica.Text = "Minimo 8 caracteres, con mayuscula, minuscula y numero.";
            this.btnGuardar.Location = new System.Drawing.Point(28, 354); this.btnGuardar.Name = "btnGuardar"; this.btnGuardar.Size = new System.Drawing.Size(500, 44); this.btnGuardar.Text = "Guardar nueva contrasena"; this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.AcceptButton = this.btnGuardar; this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20); this.ClientSize = new System.Drawing.Size(1020, 742); this.Controls.Add(this.panelCuenta); this.Controls.Add(this.lblSubtitulo); this.Controls.Add(this.lblTitulo); this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.Name = "FrmCambiarPassword"; this.Text = "Cambiar contrasena"; this.panelCuenta.ResumeLayout(false); this.panelCuenta.PerformLayout(); this.ResumeLayout(false); this.PerformLayout();
        }
    }
}
