namespace SistemaGestionNomina.UI
{
    partial class FrmHistorialEmpleado
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.ComboBox cmbEmpleado;
        private System.Windows.Forms.ComboBox cmbCampo;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Label lblResultado;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label(); this.cmbEmpleado = new System.Windows.Forms.ComboBox();
            this.cmbCampo = new System.Windows.Forms.ComboBox(); this.txtUsuario = new System.Windows.Forms.TextBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker(); this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button(); this.btnExcel = new System.Windows.Forms.Button();
            this.btnPdf = new System.Windows.Forms.Button(); this.btnCerrar = new System.Windows.Forms.Button();
            this.dgvHistorial = new System.Windows.Forms.DataGridView(); this.lblResultado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit(); this.SuspendLayout();
            this.BackColor = System.Drawing.Color.FromArgb(12, 12, 18); this.ClientSize = new System.Drawing.Size(1120, 700);
            this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmHistorialEmpleado"; this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historial laboral"; this.Load += new System.EventHandler(this.FrmHistorialEmpleado_Load);
            this.lblTitulo.AutoSize = true; this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(28, 20); this.lblTitulo.Text = "Historial laboral";
            this.cmbEmpleado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbEmpleado.Location = new System.Drawing.Point(34, 82); this.cmbEmpleado.Size = new System.Drawing.Size(220, 23);
            this.cmbCampo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbCampo.Location = new System.Drawing.Point(266, 82); this.cmbCampo.Size = new System.Drawing.Size(150, 23);
            this.txtUsuario.Location = new System.Drawing.Point(428, 82); this.txtUsuario.Size = new System.Drawing.Size(130, 23);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpDesde.Location = new System.Drawing.Point(570, 82); this.dtpDesde.ShowCheckBox = true; this.dtpDesde.Size = new System.Drawing.Size(130, 23); this.dtpDesde.Checked = false;
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpHasta.Location = new System.Drawing.Point(712, 82); this.dtpHasta.ShowCheckBox = true; this.dtpHasta.Size = new System.Drawing.Size(130, 23); this.dtpHasta.Checked = false;
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(124, 58, 237); this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnBuscar.Location = new System.Drawing.Point(854, 78); this.btnBuscar.Size = new System.Drawing.Size(78, 31); this.btnBuscar.Text = "Buscar"; this.btnBuscar.UseVisualStyleBackColor = false; this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnExcel.Location = new System.Drawing.Point(938, 78); this.btnExcel.Size = new System.Drawing.Size(64, 31); this.btnExcel.Text = "Excel"; this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            this.btnPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnPdf.Location = new System.Drawing.Point(1008, 78); this.btnPdf.Size = new System.Drawing.Size(64, 31); this.btnPdf.Text = "PDF"; this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            this.dgvHistorial.AllowUserToAddRows = false; this.dgvHistorial.AllowUserToDeleteRows = false; this.dgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill; this.dgvHistorial.BackgroundColor = System.Drawing.Color.FromArgb(18, 18, 27); this.dgvHistorial.Location = new System.Drawing.Point(34, 130); this.dgvHistorial.ReadOnly = true; this.dgvHistorial.RowHeadersVisible = false; this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; this.dgvHistorial.Size = new System.Drawing.Size(1038, 500);
            this.dgvHistorial.Columns.Add("Codigo", "Código"); this.dgvHistorial.Columns.Add("Empleado", "Empleado"); this.dgvHistorial.Columns.Add("Campo", "Campo"); this.dgvHistorial.Columns.Add("Anterior", "Anterior"); this.dgvHistorial.Columns.Add("Nuevo", "Nuevo"); this.dgvHistorial.Columns.Add("Efectiva", "Fecha efectiva"); this.dgvHistorial.Columns.Add("Registro", "Registrado"); this.dgvHistorial.Columns.Add("Usuario", "Usuario"); this.dgvHistorial.Columns.Add("Motivo", "Motivo"); this.dgvHistorial.Columns.Add("Estado", "Aplicación");
            this.lblResultado.AutoSize = true; this.lblResultado.Location = new System.Drawing.Point(34, 650); this.lblResultado.Text = "0 cambios encontrados";
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnCerrar.Location = new System.Drawing.Point(976, 644); this.btnCerrar.Size = new System.Drawing.Size(96, 32); this.btnCerrar.Text = "Cerrar"; this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            this.Controls.Add(this.lblTitulo); this.Controls.Add(this.cmbEmpleado); this.Controls.Add(this.cmbCampo); this.Controls.Add(this.txtUsuario); this.Controls.Add(this.dtpDesde); this.Controls.Add(this.dtpHasta); this.Controls.Add(this.btnBuscar); this.Controls.Add(this.btnExcel); this.Controls.Add(this.btnPdf); this.Controls.Add(this.dgvHistorial); this.Controls.Add(this.lblResultado); this.Controls.Add(this.btnCerrar);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit(); this.ResumeLayout(false); this.PerformLayout();
        }
    }
}
