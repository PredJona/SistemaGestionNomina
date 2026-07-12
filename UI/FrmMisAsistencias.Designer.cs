namespace SistemaGestionNomina.UI
{
    partial class FrmMisAsistencias
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.FlowLayoutPanel panelResumen;
        private System.Windows.Forms.Label lblPuntualesValor;
        private System.Windows.Forms.Label lblTardanzasValor;
        private System.Windows.Forms.Label lblFaltasValor;
        private System.Windows.Forms.Label lblPermisosValor;
        private System.Windows.Forms.Label lblHorasExtraValor;
        private System.Windows.Forms.DataGridView dgvAsistencias;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalida;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoras;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;

        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label(); this.panelFiltros = new System.Windows.Forms.Panel(); this.dtpDesde = new System.Windows.Forms.DateTimePicker(); this.dtpHasta = new System.Windows.Forms.DateTimePicker(); this.cmbEstado = new System.Windows.Forms.ComboBox(); this.btnFiltrar = new System.Windows.Forms.Button(); this.btnLimpiar = new System.Windows.Forms.Button(); this.panelResumen = new System.Windows.Forms.FlowLayoutPanel(); this.lblPuntualesValor = new System.Windows.Forms.Label(); this.lblTardanzasValor = new System.Windows.Forms.Label(); this.lblFaltasValor = new System.Windows.Forms.Label(); this.lblPermisosValor = new System.Windows.Forms.Label(); this.lblHorasExtraValor = new System.Windows.Forms.Label(); this.dgvAsistencias = new System.Windows.Forms.DataGridView(); this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colEntrada = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colSalida = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colHoras = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFiltros.SuspendLayout(); this.panelResumen.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)(this.dgvAsistencias)).BeginInit(); this.SuspendLayout();
            this.lblTitulo.AutoSize = true; this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold); this.lblTitulo.Location = new System.Drawing.Point(28, 20); this.lblTitulo.Text = "Mis asistencias";
            this.panelFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))); this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.panelFiltros.Controls.Add(this.dtpDesde); this.panelFiltros.Controls.Add(this.dtpHasta); this.panelFiltros.Controls.Add(this.cmbEstado); this.panelFiltros.Controls.Add(this.btnFiltrar); this.panelFiltros.Controls.Add(this.btnLimpiar); this.panelFiltros.Location = new System.Drawing.Point(34, 86); this.panelFiltros.Size = new System.Drawing.Size(952, 74);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpDesde.Location = new System.Drawing.Point(18, 22); this.dtpDesde.Name = "dtpDesde"; this.dtpDesde.ShowCheckBox = true; this.dtpDesde.Size = new System.Drawing.Size(190, 27);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpHasta.Location = new System.Drawing.Point(224, 22); this.dtpHasta.Name = "dtpHasta"; this.dtpHasta.ShowCheckBox = true; this.dtpHasta.Size = new System.Drawing.Size(190, 27);
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbEstado.Items.AddRange(new object[] { "Todos", "Puntual", "Tardanza", "Falta", "Permiso" }); this.cmbEstado.Location = new System.Drawing.Point(430, 21); this.cmbEstado.Name = "cmbEstado"; this.cmbEstado.Size = new System.Drawing.Size(176, 28);
            this.btnFiltrar.Location = new System.Drawing.Point(630, 17); this.btnFiltrar.Name = "btnFiltrar"; this.btnFiltrar.Size = new System.Drawing.Size(142, 38); this.btnFiltrar.Text = "Aplicar filtros"; this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            this.btnLimpiar.Location = new System.Drawing.Point(790, 17); this.btnLimpiar.Name = "btnLimpiar"; this.btnLimpiar.Size = new System.Drawing.Size(142, 38); this.btnLimpiar.Text = "Limpiar"; this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            this.panelResumen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))); this.panelResumen.Controls.Add(this.lblPuntualesValor); this.panelResumen.Controls.Add(this.lblTardanzasValor); this.panelResumen.Controls.Add(this.lblFaltasValor); this.panelResumen.Controls.Add(this.lblPermisosValor); this.panelResumen.Controls.Add(this.lblHorasExtraValor); this.panelResumen.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight; this.panelResumen.Location = new System.Drawing.Point(34, 178); this.panelResumen.Name = "panelResumen"; this.panelResumen.Size = new System.Drawing.Size(952, 80); this.panelResumen.WrapContents = false;
            ConfigureSummary(this.lblPuntualesValor, "Puntuales", System.Drawing.Color.FromArgb(52, 211, 153)); ConfigureSummary(this.lblTardanzasValor, "Tardanzas", System.Drawing.Color.FromArgb(251, 191, 36)); ConfigureSummary(this.lblFaltasValor, "Faltas", System.Drawing.Color.FromArgb(248, 113, 113)); ConfigureSummary(this.lblPermisosValor, "Permisos", System.Drawing.Color.FromArgb(96, 165, 250)); ConfigureSummary(this.lblHorasExtraValor, "Horas extra", System.Drawing.Color.FromArgb(167, 139, 250));
            this.dgvAsistencias.AllowUserToAddRows = false; this.dgvAsistencias.AllowUserToDeleteRows = false; this.dgvAsistencias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))); this.dgvAsistencias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill; this.dgvAsistencias.BackgroundColor = System.Drawing.Color.FromArgb(18, 19, 27); this.dgvAsistencias.BorderStyle = System.Windows.Forms.BorderStyle.None; this.dgvAsistencias.ColumnHeadersHeight = 40; this.dgvAsistencias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colFecha, this.colEntrada, this.colSalida, this.colHoras, this.colEstado }); this.dgvAsistencias.Location = new System.Drawing.Point(34, 276); this.dgvAsistencias.Name = "dgvAsistencias"; this.dgvAsistencias.ReadOnly = true; this.dgvAsistencias.RowHeadersVisible = false; this.dgvAsistencias.RowTemplate.Height = 36; this.dgvAsistencias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; this.dgvAsistencias.Size = new System.Drawing.Size(952, 430);
            this.colFecha.HeaderText = "Fecha"; this.colEntrada.HeaderText = "Entrada"; this.colSalida.HeaderText = "Salida"; this.colHoras.HeaderText = "Horas trabajadas"; this.colEstado.HeaderText = "Estado";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20); this.ClientSize = new System.Drawing.Size(1020, 742); this.Controls.Add(this.dgvAsistencias); this.Controls.Add(this.panelResumen); this.Controls.Add(this.panelFiltros); this.Controls.Add(this.lblTitulo); this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.Name = "FrmMisAsistencias"; this.Text = "Mis asistencias"; this.Load += new System.EventHandler(this.FrmMisAsistencias_Load); this.panelFiltros.ResumeLayout(false); this.panelResumen.ResumeLayout(false); ((System.ComponentModel.ISupportInitialize)(this.dgvAsistencias)).EndInit(); this.ResumeLayout(false); this.PerformLayout();
        }

        private static void ConfigureSummary(System.Windows.Forms.Label label, string title, System.Drawing.Color color) { label.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); label.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold); label.ForeColor = color; label.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0); label.Size = new System.Drawing.Size(178, 76); label.Text = "0\n" + title; label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; }
    }
}
