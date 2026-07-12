namespace SistemaGestionNomina.UI
{
    partial class FrmMiPerfil
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.TableLayoutPanel tablePerfil;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblNombreValor;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblCodigoValor;
        private System.Windows.Forms.Label lblCedula;
        private System.Windows.Forms.Label lblCedulaValor;
        private System.Windows.Forms.Label lblCargo;
        private System.Windows.Forms.Label lblCargoValor;
        private System.Windows.Forms.Label lblDepartamento;
        private System.Windows.Forms.Label lblDepartamentoValor;
        private System.Windows.Forms.Label lblSalario;
        private System.Windows.Forms.Label lblSalarioValor;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblEstadoValor;
        private System.Windows.Forms.Label lblIngreso;
        private System.Windows.Forms.Label lblIngresoValor;

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label(); this.lblSubtitulo = new System.Windows.Forms.Label(); this.tablePerfil = new System.Windows.Forms.TableLayoutPanel();
            this.lblNombre = new System.Windows.Forms.Label(); this.lblNombreValor = new System.Windows.Forms.Label(); this.lblCodigo = new System.Windows.Forms.Label(); this.lblCodigoValor = new System.Windows.Forms.Label(); this.lblCedula = new System.Windows.Forms.Label(); this.lblCedulaValor = new System.Windows.Forms.Label(); this.lblCargo = new System.Windows.Forms.Label(); this.lblCargoValor = new System.Windows.Forms.Label(); this.lblDepartamento = new System.Windows.Forms.Label(); this.lblDepartamentoValor = new System.Windows.Forms.Label(); this.lblSalario = new System.Windows.Forms.Label(); this.lblSalarioValor = new System.Windows.Forms.Label(); this.lblEstado = new System.Windows.Forms.Label(); this.lblEstadoValor = new System.Windows.Forms.Label(); this.lblIngreso = new System.Windows.Forms.Label(); this.lblIngresoValor = new System.Windows.Forms.Label();
            this.tablePerfil.SuspendLayout(); this.SuspendLayout();
            this.lblTitulo.AutoSize = true; this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold); this.lblTitulo.Location = new System.Drawing.Point(32, 24); this.lblTitulo.Text = "Mi perfil laboral";
            this.lblSubtitulo.AutoSize = true; this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblSubtitulo.Location = new System.Drawing.Point(36, 78); this.lblSubtitulo.Text = "Informacion registrada por Recursos Humanos.";
            this.tablePerfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))); this.tablePerfil.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.tablePerfil.ColumnCount = 2; this.tablePerfil.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F)); this.tablePerfil.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68F)); this.tablePerfil.Controls.Add(this.lblNombre, 0, 0); this.tablePerfil.Controls.Add(this.lblNombreValor, 1, 0); this.tablePerfil.Controls.Add(this.lblCodigo, 0, 1); this.tablePerfil.Controls.Add(this.lblCodigoValor, 1, 1); this.tablePerfil.Controls.Add(this.lblCedula, 0, 2); this.tablePerfil.Controls.Add(this.lblCedulaValor, 1, 2); this.tablePerfil.Controls.Add(this.lblCargo, 0, 3); this.tablePerfil.Controls.Add(this.lblCargoValor, 1, 3); this.tablePerfil.Controls.Add(this.lblDepartamento, 0, 4); this.tablePerfil.Controls.Add(this.lblDepartamentoValor, 1, 4); this.tablePerfil.Controls.Add(this.lblSalario, 0, 5); this.tablePerfil.Controls.Add(this.lblSalarioValor, 1, 5); this.tablePerfil.Controls.Add(this.lblEstado, 0, 6); this.tablePerfil.Controls.Add(this.lblEstadoValor, 1, 6); this.tablePerfil.Controls.Add(this.lblIngreso, 0, 7); this.tablePerfil.Controls.Add(this.lblIngresoValor, 1, 7); this.tablePerfil.Location = new System.Drawing.Point(38, 126); this.tablePerfil.Name = "tablePerfil"; this.tablePerfil.Padding = new System.Windows.Forms.Padding(24); this.tablePerfil.RowCount = 8; for (int i = 0; i < 8; i++) this.tablePerfil.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F)); this.tablePerfil.Size = new System.Drawing.Size(944, 514);
            ConfigureCaption(this.lblNombre, "Nombre completo"); ConfigureValue(this.lblNombreValor); ConfigureCaption(this.lblCodigo, "Codigo de empleado"); ConfigureValue(this.lblCodigoValor); ConfigureCaption(this.lblCedula, "Cedula"); ConfigureValue(this.lblCedulaValor); ConfigureCaption(this.lblCargo, "Cargo"); ConfigureValue(this.lblCargoValor); ConfigureCaption(this.lblDepartamento, "Departamento"); ConfigureValue(this.lblDepartamentoValor); ConfigureCaption(this.lblSalario, "Salario base"); ConfigureValue(this.lblSalarioValor); ConfigureCaption(this.lblEstado, "Estado laboral"); ConfigureValue(this.lblEstadoValor); ConfigureCaption(this.lblIngreso, "Fecha de ingreso"); ConfigureValue(this.lblIngresoValor);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20); this.ClientSize = new System.Drawing.Size(1020, 742); this.Controls.Add(this.tablePerfil); this.Controls.Add(this.lblSubtitulo); this.Controls.Add(this.lblTitulo); this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.Name = "FrmMiPerfil"; this.Text = "Mi perfil"; this.Load += new System.EventHandler(this.FrmMiPerfil_Load); this.tablePerfil.ResumeLayout(false); this.ResumeLayout(false); this.PerformLayout();
        }

        private static void ConfigureCaption(System.Windows.Forms.Label label, string text) { label.Dock = System.Windows.Forms.DockStyle.Fill; label.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); label.Text = text; label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft; }
        private static void ConfigureValue(System.Windows.Forms.Label label) { label.AutoEllipsis = true; label.Dock = System.Windows.Forms.DockStyle.Fill; label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); label.Text = "-"; label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft; }
    }
}
