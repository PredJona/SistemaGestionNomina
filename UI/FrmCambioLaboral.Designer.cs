namespace SistemaGestionNomina.UI
{
    partial class FrmCambioLaboral
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEmpleado;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFechaEfectiva;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEmpleado = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFechaEfectiva = new System.Windows.Forms.DateTimePicker();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.BackColor = System.Drawing.Color.FromArgb(18, 18, 27);
            this.ClientSize = new System.Drawing.Size(520, 330);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCambioLaboral";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cambio laboral";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(28, 24);
            this.lblTitulo.Text = "Fecha efectiva del cambio";
            this.lblEmpleado.AutoSize = true;
            this.lblEmpleado.ForeColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.lblEmpleado.Location = new System.Drawing.Point(31, 72);
            this.lblEmpleado.Text = "Empleado seleccionado";
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(31, 112);
            this.lblFecha.Text = "Fecha efectiva";
            this.dtpFechaEfectiva.Location = new System.Drawing.Point(34, 134);
            this.dtpFechaEfectiva.Size = new System.Drawing.Size(210, 23);
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Location = new System.Drawing.Point(31, 176);
            this.lblMotivo.Text = "Motivo obligatorio";
            this.txtMotivo.Location = new System.Drawing.Point(34, 198);
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Size = new System.Drawing.Size(450, 68);
            this.txtMotivo.MaxLength = 300;
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(124, 58, 237);
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Location = new System.Drawing.Point(294, 282);
            this.btnAceptar.Size = new System.Drawing.Size(92, 32);
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Location = new System.Drawing.Point(392, 282);
            this.btnCancelar.Size = new System.Drawing.Size(92, 32);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEmpleado);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtpFechaEfectiva);
            this.Controls.Add(this.lblMotivo);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
