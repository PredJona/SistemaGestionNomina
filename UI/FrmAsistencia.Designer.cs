namespace SistemaGestionNomina.UI
{
    partial class FrmAsistencia
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelRegistro;
        private System.Windows.Forms.ComboBox cmbEmpleado;
        private System.Windows.Forms.Label lblEmpleado;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblEntrada;
        private System.Windows.Forms.Label lblSalida;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.DateTimePicker dtpHoraEntrada;
        private System.Windows.Forms.DateTimePicker dtpHoraSalida;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnCargarReloj;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.DateTimePicker dtpFiltroInicio;
        private System.Windows.Forms.DateTimePicker dtpFiltroFin;
        private System.Windows.Forms.ComboBox cmbFiltroEmpleado;
        private System.Windows.Forms.ComboBox cmbFiltroEstado;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Panel cardPresentes;
        private System.Windows.Forms.Panel cardAusentes;
        private System.Windows.Forms.Panel cardTardanzas;
        private System.Windows.Forms.Panel cardPorcentaje;
        private System.Windows.Forms.Label lblPresentesTitulo;
        private System.Windows.Forms.Label lblAusentesTitulo;
        private System.Windows.Forms.Label lblTardanzasTitulo;
        private System.Windows.Forms.Label lblPorcentajeTitulo;
        private System.Windows.Forms.Label lblPresentesValor;
        private System.Windows.Forms.Label lblAusentesValor;
        private System.Windows.Forms.Label lblTardanzasValor;
        private System.Windows.Forms.Label lblPorcentajeValor;
        private System.Windows.Forms.DataGridView dgvAsistencia;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelRegistro = new System.Windows.Forms.Panel();
            this.cmbEmpleado = new System.Windows.Forms.ComboBox();
            this.lblEmpleado = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblEntrada = new System.Windows.Forms.Label();
            this.lblSalida = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.dtpHoraEntrada = new System.Windows.Forms.DateTimePicker();
            this.dtpHoraSalida = new System.Windows.Forms.DateTimePicker();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnCargarReloj = new System.Windows.Forms.Button();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.dtpFiltroInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpFiltroFin = new System.Windows.Forms.DateTimePicker();
            this.cmbFiltroEmpleado = new System.Windows.Forms.ComboBox();
            this.cmbFiltroEstado = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.cardPresentes = new System.Windows.Forms.Panel();
            this.lblPresentesTitulo = new System.Windows.Forms.Label();
            this.lblPresentesValor = new System.Windows.Forms.Label();
            this.cardAusentes = new System.Windows.Forms.Panel();
            this.lblAusentesTitulo = new System.Windows.Forms.Label();
            this.lblAusentesValor = new System.Windows.Forms.Label();
            this.cardTardanzas = new System.Windows.Forms.Panel();
            this.lblTardanzasTitulo = new System.Windows.Forms.Label();
            this.lblTardanzasValor = new System.Windows.Forms.Label();
            this.cardPorcentaje = new System.Windows.Forms.Panel();
            this.lblPorcentajeTitulo = new System.Windows.Forms.Label();
            this.lblPorcentajeValor = new System.Windows.Forms.Label();
            this.dgvAsistencia = new System.Windows.Forms.DataGridView();
            this.panelRegistro.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            this.cardPresentes.SuspendLayout();
            this.cardAusentes.SuspendLayout();
            this.cardTardanzas.SuspendLayout();
            this.cardPorcentaje.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsistencia)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblTitulo.Location = new System.Drawing.Point(34, 24);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(500, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Control de Asistencia";
            // 
            // panelRegistro
            // 
            this.panelRegistro.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.panelRegistro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRegistro.Controls.Add(this.lblEmpleado);
            this.panelRegistro.Controls.Add(this.lblFecha);
            this.panelRegistro.Controls.Add(this.lblEntrada);
            this.panelRegistro.Controls.Add(this.lblSalida);
            this.panelRegistro.Controls.Add(this.lblEstado);
            this.panelRegistro.Controls.Add(this.cmbEmpleado);
            this.panelRegistro.Controls.Add(this.dtpFecha);
            this.panelRegistro.Controls.Add(this.dtpHoraEntrada);
            this.panelRegistro.Controls.Add(this.dtpHoraSalida);
            this.panelRegistro.Controls.Add(this.cmbEstado);
            this.panelRegistro.Controls.Add(this.btnRegistrar);
            this.panelRegistro.Controls.Add(this.btnCargarReloj);
            this.panelRegistro.Location = new System.Drawing.Point(36, 92);
            this.panelRegistro.Name = "panelRegistro";
            this.panelRegistro.Size = new System.Drawing.Size(948, 92);
            this.panelRegistro.TabIndex = 1;
            // 
            // cmbEmpleado
            // 
            this.cmbEmpleado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmpleado.Location = new System.Drawing.Point(18, 44);
            this.cmbEmpleado.Name = "cmbEmpleado";
            this.cmbEmpleado.Size = new System.Drawing.Size(240, 23);
            this.cmbEmpleado.TabIndex = 0;
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(270, 44);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(110, 23);
            this.dtpFecha.TabIndex = 1;
            // 
            // dtpHoraEntrada
            // 
            this.dtpHoraEntrada.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraEntrada.Location = new System.Drawing.Point(392, 44);
            this.dtpHoraEntrada.Name = "dtpHoraEntrada";
            this.dtpHoraEntrada.ShowUpDown = true;
            this.dtpHoraEntrada.Size = new System.Drawing.Size(100, 23);
            this.dtpHoraEntrada.TabIndex = 2;
            // 
            // dtpHoraSalida
            // 
            this.dtpHoraSalida.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraSalida.Location = new System.Drawing.Point(504, 44);
            this.dtpHoraSalida.Name = "dtpHoraSalida";
            this.dtpHoraSalida.ShowUpDown = true;
            this.dtpHoraSalida.Size = new System.Drawing.Size(100, 23);
            this.dtpHoraSalida.TabIndex = 3;
            // 
            // cmbEstado
            // 
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Items.AddRange(new object[] { "Puntual", "Tardanza", "Falta", "Permiso" });
            this.cmbEstado.Location = new System.Drawing.Point(616, 44);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.SelectedIndex = 0;
            this.cmbEstado.Size = new System.Drawing.Size(120, 23);
            this.cmbEstado.TabIndex = 4;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.Location = new System.Drawing.Point(750, 38);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(90, 30);
            this.btnRegistrar.TabIndex = 5;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // btnCargarReloj
            // 
            this.btnCargarReloj.BackColor = System.Drawing.Color.Black;
            this.btnCargarReloj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargarReloj.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnCargarReloj.Location = new System.Drawing.Point(846, 38);
            this.btnCargarReloj.Name = "btnCargarReloj";
            this.btnCargarReloj.Size = new System.Drawing.Size(86, 30);
            this.btnCargarReloj.TabIndex = 6;
            this.btnCargarReloj.Text = "Reloj";
            this.btnCargarReloj.UseVisualStyleBackColor = false;
            this.btnCargarReloj.Click += new System.EventHandler(this.btnCargarReloj_Click);
            this.lblEmpleado.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblEmpleado.Location = new System.Drawing.Point(18, 22);
            this.lblEmpleado.Name = "lblEmpleado";
            this.lblEmpleado.Size = new System.Drawing.Size(120, 18);
            this.lblEmpleado.Text = "Empleado";
            this.lblFecha.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblFecha.Location = new System.Drawing.Point(270, 22);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(80, 18);
            this.lblFecha.Text = "Fecha";
            this.lblEntrada.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblEntrada.Location = new System.Drawing.Point(392, 22);
            this.lblEntrada.Name = "lblEntrada";
            this.lblEntrada.Size = new System.Drawing.Size(90, 18);
            this.lblEntrada.Text = "Entrada";
            this.lblSalida.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblSalida.Location = new System.Drawing.Point(504, 22);
            this.lblSalida.Name = "lblSalida";
            this.lblSalida.Size = new System.Drawing.Size(90, 18);
            this.lblSalida.Text = "Salida";
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblEstado.Location = new System.Drawing.Point(616, 22);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(90, 18);
            this.lblEstado.Text = "Estado";
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.panelFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltros.Controls.Add(this.dtpFiltroInicio);
            this.panelFiltros.Controls.Add(this.dtpFiltroFin);
            this.panelFiltros.Controls.Add(this.cmbFiltroEmpleado);
            this.panelFiltros.Controls.Add(this.cmbFiltroEstado);
            this.panelFiltros.Controls.Add(this.btnFiltrar);
            this.panelFiltros.Controls.Add(this.btnExportarExcel);
            this.panelFiltros.Controls.Add(this.btnExportarPdf);
            this.panelFiltros.Location = new System.Drawing.Point(36, 206);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(948, 80);
            this.panelFiltros.TabIndex = 2;
            // 
            // dtpFiltroInicio
            // 
            this.dtpFiltroInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFiltroInicio.Location = new System.Drawing.Point(18, 28);
            this.dtpFiltroInicio.Name = "dtpFiltroInicio";
            this.dtpFiltroInicio.Size = new System.Drawing.Size(110, 23);
            this.dtpFiltroInicio.TabIndex = 0;
            // 
            // dtpFiltroFin
            // 
            this.dtpFiltroFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFiltroFin.Location = new System.Drawing.Point(140, 28);
            this.dtpFiltroFin.Name = "dtpFiltroFin";
            this.dtpFiltroFin.Size = new System.Drawing.Size(110, 23);
            this.dtpFiltroFin.TabIndex = 1;
            // 
            // cmbFiltroEmpleado
            // 
            this.cmbFiltroEmpleado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroEmpleado.Location = new System.Drawing.Point(262, 28);
            this.cmbFiltroEmpleado.Name = "cmbFiltroEmpleado";
            this.cmbFiltroEmpleado.Size = new System.Drawing.Size(220, 23);
            this.cmbFiltroEmpleado.TabIndex = 2;
            // 
            // cmbFiltroEstado
            // 
            this.cmbFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Puntual", "Tardanza", "Falta", "Permiso" });
            this.cmbFiltroEstado.Location = new System.Drawing.Point(494, 28);
            this.cmbFiltroEstado.Name = "cmbFiltroEstado";
            this.cmbFiltroEstado.SelectedIndex = 0;
            this.cmbFiltroEstado.Size = new System.Drawing.Size(120, 23);
            this.cmbFiltroEstado.TabIndex = 3;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Location = new System.Drawing.Point(630, 24);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(80, 30);
            this.btnFiltrar.TabIndex = 4;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.Black;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarExcel.Location = new System.Drawing.Point(724, 24);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.BackColor = System.Drawing.Color.Black;
            this.btnExportarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPdf.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarPdf.Location = new System.Drawing.Point(816, 24);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(80, 30);
            this.btnExportarPdf.TabIndex = 6;
            this.btnExportarPdf.Text = "PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = false;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // summary cards
            // 
            this.cardPresentes.BackColor = System.Drawing.Color.Black;
            this.cardPresentes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPresentes.Controls.Add(this.lblPresentesTitulo);
            this.cardPresentes.Controls.Add(this.lblPresentesValor);
            this.cardPresentes.Location = new System.Drawing.Point(36, 304);
            this.cardPresentes.Name = "cardPresentes";
            this.cardPresentes.Size = new System.Drawing.Size(222, 64);
            this.cardPresentes.TabIndex = 3;
            this.lblPresentesTitulo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblPresentesTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblPresentesTitulo.Name = "lblPresentesTitulo";
            this.lblPresentesTitulo.Size = new System.Drawing.Size(160, 18);
            this.lblPresentesTitulo.TabIndex = 0;
            this.lblPresentesTitulo.Text = "Presentes";
            this.lblPresentesValor.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPresentesValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblPresentesValor.Location = new System.Drawing.Point(16, 28);
            this.lblPresentesValor.Name = "lblPresentesValor";
            this.lblPresentesValor.Size = new System.Drawing.Size(170, 28);
            this.lblPresentesValor.TabIndex = 1;
            this.lblPresentesValor.Text = "0";
            this.cardAusentes.BackColor = System.Drawing.Color.Black;
            this.cardAusentes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAusentes.Controls.Add(this.lblAusentesTitulo);
            this.cardAusentes.Controls.Add(this.lblAusentesValor);
            this.cardAusentes.Location = new System.Drawing.Point(278, 304);
            this.cardAusentes.Name = "cardAusentes";
            this.cardAusentes.Size = new System.Drawing.Size(222, 64);
            this.cardAusentes.TabIndex = 4;
            this.lblAusentesTitulo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblAusentesTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblAusentesTitulo.Name = "lblAusentesTitulo";
            this.lblAusentesTitulo.Size = new System.Drawing.Size(160, 18);
            this.lblAusentesTitulo.TabIndex = 0;
            this.lblAusentesTitulo.Text = "Ausentes";
            this.lblAusentesValor.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAusentesValor.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.lblAusentesValor.Location = new System.Drawing.Point(16, 28);
            this.lblAusentesValor.Name = "lblAusentesValor";
            this.lblAusentesValor.Size = new System.Drawing.Size(170, 28);
            this.lblAusentesValor.TabIndex = 1;
            this.lblAusentesValor.Text = "0";
            this.cardTardanzas.BackColor = System.Drawing.Color.Black;
            this.cardTardanzas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTardanzas.Controls.Add(this.lblTardanzasTitulo);
            this.cardTardanzas.Controls.Add(this.lblTardanzasValor);
            this.cardTardanzas.Location = new System.Drawing.Point(520, 304);
            this.cardTardanzas.Name = "cardTardanzas";
            this.cardTardanzas.Size = new System.Drawing.Size(222, 64);
            this.cardTardanzas.TabIndex = 5;
            this.lblTardanzasTitulo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblTardanzasTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblTardanzasTitulo.Name = "lblTardanzasTitulo";
            this.lblTardanzasTitulo.Size = new System.Drawing.Size(160, 18);
            this.lblTardanzasTitulo.TabIndex = 0;
            this.lblTardanzasTitulo.Text = "Tardanzas";
            this.lblTardanzasValor.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTardanzasValor.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            this.lblTardanzasValor.Location = new System.Drawing.Point(16, 28);
            this.lblTardanzasValor.Name = "lblTardanzasValor";
            this.lblTardanzasValor.Size = new System.Drawing.Size(170, 28);
            this.lblTardanzasValor.TabIndex = 1;
            this.lblTardanzasValor.Text = "0";
            this.cardPorcentaje.BackColor = System.Drawing.Color.Black;
            this.cardPorcentaje.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPorcentaje.Controls.Add(this.lblPorcentajeTitulo);
            this.cardPorcentaje.Controls.Add(this.lblPorcentajeValor);
            this.cardPorcentaje.Location = new System.Drawing.Point(762, 304);
            this.cardPorcentaje.Name = "cardPorcentaje";
            this.cardPorcentaje.Size = new System.Drawing.Size(222, 64);
            this.cardPorcentaje.TabIndex = 6;
            this.lblPorcentajeTitulo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblPorcentajeTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblPorcentajeTitulo.Name = "lblPorcentajeTitulo";
            this.lblPorcentajeTitulo.Size = new System.Drawing.Size(160, 18);
            this.lblPorcentajeTitulo.TabIndex = 0;
            this.lblPorcentajeTitulo.Text = "Asistencia";
            this.lblPorcentajeValor.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPorcentajeValor.ForeColor = System.Drawing.Color.FromArgb(139, 92, 246);
            this.lblPorcentajeValor.Location = new System.Drawing.Point(16, 28);
            this.lblPorcentajeValor.Name = "lblPorcentajeValor";
            this.lblPorcentajeValor.Size = new System.Drawing.Size(170, 28);
            this.lblPorcentajeValor.TabIndex = 1;
            this.lblPorcentajeValor.Text = "0%";
            // 
            // dgvAsistencia
            // 
            this.dgvAsistencia.AllowUserToAddRows = false;
            this.dgvAsistencia.AllowUserToDeleteRows = false;
            this.dgvAsistencia.BackgroundColor = System.Drawing.Color.Black;
            this.dgvAsistencia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAsistencia.Location = new System.Drawing.Point(36, 392);
            this.dgvAsistencia.Name = "dgvAsistencia";
            this.dgvAsistencia.ReadOnly = true;
            this.dgvAsistencia.RowHeadersVisible = false;
            this.dgvAsistencia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAsistencia.Size = new System.Drawing.Size(948, 286);
            this.dgvAsistencia.TabIndex = 7;
            this.dgvAsistencia.Columns.Add("Empleado", "Empleado");
            this.dgvAsistencia.Columns.Add("Fecha", "Fecha");
            this.dgvAsistencia.Columns.Add("Entrada", "Entrada");
            this.dgvAsistencia.Columns.Add("Salida", "Salida");
            this.dgvAsistencia.Columns.Add("Horas", "Horas");
            this.dgvAsistencia.Columns.Add("Estado", "Estado");
            // 
            // FrmAsistencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.dgvAsistencia);
            this.Controls.Add(this.cardPorcentaje);
            this.Controls.Add(this.cardTardanzas);
            this.Controls.Add(this.cardAusentes);
            this.Controls.Add(this.cardPresentes);
            this.Controls.Add(this.panelFiltros);
            this.Controls.Add(this.panelRegistro);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmAsistencia";
            this.Text = "Asistencia";
            this.Load += new System.EventHandler(this.FrmAsistencia_Load);
            this.panelRegistro.ResumeLayout(false);
            this.panelFiltros.ResumeLayout(false);
            this.cardPresentes.ResumeLayout(false);
            this.cardAusentes.ResumeLayout(false);
            this.cardTardanzas.ResumeLayout(false);
            this.cardPorcentaje.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAsistencia)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
