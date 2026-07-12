namespace SistemaGestionNomina.UI
{
    partial class FrmAuditoria
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.ComboBox cmbModulo;
        private System.Windows.Forms.Label lblAccion;
        private System.Windows.Forms.ComboBox cmbAccion;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvAuditoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetalle;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblPagina;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblModulo = new System.Windows.Forms.Label();
            this.cmbModulo = new System.Windows.Forms.ComboBox();
            this.lblAccion = new System.Windows.Forms.Label();
            this.cmbAccion = new System.Windows.Forms.ComboBox();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvAuditoria = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetalle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditoria)).BeginInit();
            this.SuspendLayout();
            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblTitulo.Location = new System.Drawing.Point(28, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(329, 50);
            this.lblTitulo.Text = "Auditoría del sistema";
            // panelFiltros
            this.panelFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.panelFiltros.Controls.Add(this.lblUsuario);
            this.panelFiltros.Controls.Add(this.txtUsuario);
            this.panelFiltros.Controls.Add(this.lblModulo);
            this.panelFiltros.Controls.Add(this.cmbModulo);
            this.panelFiltros.Controls.Add(this.lblAccion);
            this.panelFiltros.Controls.Add(this.cmbAccion);
            this.panelFiltros.Controls.Add(this.lblDetalle);
            this.panelFiltros.Controls.Add(this.txtDetalle);
            this.panelFiltros.Controls.Add(this.lblDesde);
            this.panelFiltros.Controls.Add(this.dtpDesde);
            this.panelFiltros.Controls.Add(this.lblHasta);
            this.panelFiltros.Controls.Add(this.dtpHasta);
            this.panelFiltros.Controls.Add(this.btnBuscar);
            this.panelFiltros.Controls.Add(this.btnLimpiar);
            this.panelFiltros.Location = new System.Drawing.Point(32, 82);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(956, 136);
            // labels
            this.lblUsuario.AutoSize = true; this.lblUsuario.Location = new System.Drawing.Point(18, 14); this.lblUsuario.Text = "Usuario";
            this.lblModulo.AutoSize = true; this.lblModulo.Location = new System.Drawing.Point(206, 14); this.lblModulo.Text = "Módulo";
            this.lblAccion.AutoSize = true; this.lblAccion.Location = new System.Drawing.Point(394, 14); this.lblAccion.Text = "Acción";
            this.lblDetalle.AutoSize = true; this.lblDetalle.Location = new System.Drawing.Point(582, 14); this.lblDetalle.Text = "Detalle";
            this.lblDesde.AutoSize = true; this.lblDesde.Location = new System.Drawing.Point(18, 75); this.lblDesde.Text = "Desde";
            this.lblHasta.AutoSize = true; this.lblHasta.Location = new System.Drawing.Point(238, 75); this.lblHasta.Text = "Hasta";
            // inputs
            this.txtUsuario.Location = new System.Drawing.Point(18, 36); this.txtUsuario.Size = new System.Drawing.Size(170, 27);
            this.cmbModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbModulo.Location = new System.Drawing.Point(206, 36); this.cmbModulo.Size = new System.Drawing.Size(170, 28);
            this.cmbAccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbAccion.Location = new System.Drawing.Point(394, 36); this.cmbAccion.Size = new System.Drawing.Size(170, 28);
            this.txtDetalle.Location = new System.Drawing.Point(582, 36); this.txtDetalle.Size = new System.Drawing.Size(350, 27);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpDesde.Location = new System.Drawing.Point(18, 96); this.dtpDesde.ShowCheckBox = true; this.dtpDesde.Size = new System.Drawing.Size(202, 27);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short; this.dtpHasta.Location = new System.Drawing.Point(238, 96); this.dtpHasta.ShowCheckBox = true; this.dtpHasta.Size = new System.Drawing.Size(202, 27);
            // buttons
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(124, 58, 237); this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnBuscar.ForeColor = System.Drawing.Color.White; this.btnBuscar.Location = new System.Drawing.Point(674, 88); this.btnBuscar.Size = new System.Drawing.Size(122, 36); this.btnBuscar.Text = "Buscar"; this.btnBuscar.UseVisualStyleBackColor = false; this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(42, 43, 54); this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnLimpiar.ForeColor = System.Drawing.Color.White; this.btnLimpiar.Location = new System.Drawing.Point(810, 88); this.btnLimpiar.Size = new System.Drawing.Size(122, 36); this.btnLimpiar.Text = "Limpiar"; this.btnLimpiar.UseVisualStyleBackColor = false; this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // dgvAuditoria
            this.dgvAuditoria.AllowUserToAddRows = false;
            this.dgvAuditoria.AllowUserToDeleteRows = false;
            this.dgvAuditoria.AllowUserToResizeRows = false;
            this.dgvAuditoria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAuditoria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAuditoria.BackgroundColor = System.Drawing.Color.FromArgb(18, 19, 27);
            this.dgvAuditoria.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(42, 43, 54); headerStyle.ForeColor = System.Drawing.Color.White; headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(42, 43, 54); headerStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvAuditoria.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvAuditoria.ColumnHeadersHeight = 40;
            this.dgvAuditoria.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colFecha, this.colUsuario, this.colModulo, this.colAccion, this.colDetalle });
            cellStyle.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); cellStyle.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); cellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(76, 59, 118); cellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvAuditoria.DefaultCellStyle = cellStyle;
            this.dgvAuditoria.EnableHeadersVisualStyles = false;
            this.dgvAuditoria.Location = new System.Drawing.Point(32, 236);
            this.dgvAuditoria.MultiSelect = false;
            this.dgvAuditoria.ReadOnly = true;
            this.dgvAuditoria.RowHeadersVisible = false;
            this.dgvAuditoria.RowTemplate.Height = 32;
            this.dgvAuditoria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAuditoria.Size = new System.Drawing.Size(956, 418);
            this.colFecha.HeaderText = "Fecha"; this.colFecha.FillWeight = 95F; this.colFecha.ReadOnly = true;
            this.colUsuario.HeaderText = "Usuario"; this.colUsuario.FillWeight = 75F; this.colUsuario.ReadOnly = true;
            this.colModulo.HeaderText = "Módulo"; this.colModulo.FillWeight = 75F; this.colModulo.ReadOnly = true;
            this.colAccion.HeaderText = "Acción"; this.colAccion.FillWeight = 95F; this.colAccion.ReadOnly = true;
            this.colDetalle.HeaderText = "Detalle"; this.colDetalle.FillWeight = 190F; this.colDetalle.ReadOnly = true;
            // pagination
            this.btnAnterior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))); this.btnAnterior.Location = new System.Drawing.Point(764, 670); this.btnAnterior.Size = new System.Drawing.Size(100, 34); this.btnAnterior.Text = "Anterior"; this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            this.btnSiguiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))); this.btnSiguiente.Location = new System.Drawing.Point(888, 670); this.btnSiguiente.Size = new System.Drawing.Size(100, 34); this.btnSiguiente.Text = "Siguiente"; this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            this.lblPagina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))); this.lblPagina.Location = new System.Drawing.Point(32, 675); this.lblPagina.Size = new System.Drawing.Size(300, 24); this.lblPagina.Text = "Página 1";
            // FrmAuditoria
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 720);
            this.Controls.Add(this.lblPagina);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.dgvAuditoria);
            this.Controls.Add(this.panelFiltros);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmAuditoria";
            this.Text = "Auditoría";
            this.Load += new System.EventHandler(this.FrmAuditoria_Load);
            this.panelFiltros.ResumeLayout(false);
            this.panelFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditoria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
