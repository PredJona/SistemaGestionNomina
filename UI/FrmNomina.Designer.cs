namespace SistemaGestionNomina.UI
{
    partial class FrmNomina
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelParametros;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.Label lblDepartamento;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.ComboBox cmbDepartamento;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Panel cardTotales;
        private System.Windows.Forms.Label lblTotalNeto;
        private System.Windows.Forms.Label lblProcesados;
        private System.Windows.Forms.Label lblTotalIngresos;
        private System.Windows.Forms.Label lblTotalDeducciones;
        private System.Windows.Forms.Button btnConfirmarPago;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.DataGridView dgvNominaDetalle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelParametros = new System.Windows.Forms.Panel();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.lblDepartamento = new System.Windows.Forms.Label();
            this.lblResumen = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.cmbDepartamento = new System.Windows.Forms.ComboBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.cardTotales = new System.Windows.Forms.Panel();
            this.lblTotalNeto = new System.Windows.Forms.Label();
            this.lblProcesados = new System.Windows.Forms.Label();
            this.lblTotalIngresos = new System.Windows.Forms.Label();
            this.lblTotalDeducciones = new System.Windows.Forms.Label();
            this.btnConfirmarPago = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.dgvNominaDetalle = new System.Windows.Forms.DataGridView();
            this.panelParametros.SuspendLayout();
            this.cardTotales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNominaDetalle)).BeginInit();
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
            this.lblTitulo.Text = "Cálculo de Nómina";
            // 
            // panelParametros
            // 
            this.panelParametros.BackColor = System.Drawing.Color.Black;
            this.panelParametros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParametros.Controls.Add(this.lblPeriodo);
            this.panelParametros.Controls.Add(this.lblFechaInicio);
            this.panelParametros.Controls.Add(this.lblFechaFin);
            this.panelParametros.Controls.Add(this.lblDepartamento);
            this.panelParametros.Controls.Add(this.cmbPeriodo);
            this.panelParametros.Controls.Add(this.dtpFechaInicio);
            this.panelParametros.Controls.Add(this.dtpFechaFin);
            this.panelParametros.Controls.Add(this.cmbDepartamento);
            this.panelParametros.Controls.Add(this.btnCalcular);
            this.panelParametros.Location = new System.Drawing.Point(36, 96);
            this.panelParametros.Name = "panelParametros";
            this.panelParametros.Size = new System.Drawing.Size(330, 250);
            this.panelParametros.TabIndex = 1;
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.cmbPeriodo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.cmbPeriodo.Items.AddRange(new object[] { "Nómina mensual", "Primera quincena", "Segunda quincena" });
            this.cmbPeriodo.Location = new System.Drawing.Point(22, 50);
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(280, 23);
            this.cmbPeriodo.TabIndex = 0;
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(22, 104);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(130, 23);
            this.dtpFechaInicio.TabIndex = 1;
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(172, 104);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(130, 23);
            this.dtpFechaFin.TabIndex = 2;
            // 
            // cmbDepartamento
            // 
            this.cmbDepartamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartamento.Location = new System.Drawing.Point(22, 160);
            this.cmbDepartamento.Name = "cmbDepartamento";
            this.cmbDepartamento.Size = new System.Drawing.Size(280, 23);
            this.cmbDepartamento.TabIndex = 3;
            // 
            // btnCalcular
            // 
            this.btnCalcular.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalcular.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalcular.Location = new System.Drawing.Point(22, 198);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(280, 38);
            this.btnCalcular.TabIndex = 4;
            this.btnCalcular.Text = "Calcular Nómina";
            this.btnCalcular.UseVisualStyleBackColor = false;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            this.lblPeriodo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblPeriodo.Location = new System.Drawing.Point(22, 28);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(130, 18);
            this.lblPeriodo.TabIndex = 10;
            this.lblPeriodo.Text = "Periodo";
            this.lblFechaInicio.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblFechaInicio.Location = new System.Drawing.Point(22, 82);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(130, 18);
            this.lblFechaInicio.TabIndex = 11;
            this.lblFechaInicio.Text = "Fecha inicio";
            this.lblFechaFin.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblFechaFin.Location = new System.Drawing.Point(172, 82);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(130, 18);
            this.lblFechaFin.TabIndex = 12;
            this.lblFechaFin.Text = "Fecha fin";
            this.lblDepartamento.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblDepartamento.Location = new System.Drawing.Point(22, 138);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(130, 18);
            this.lblDepartamento.TabIndex = 13;
            this.lblDepartamento.Text = "Departamento";
            // 
            // cardTotales
            // 
            this.cardTotales.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.cardTotales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotales.Controls.Add(this.lblResumen);
            this.cardTotales.Controls.Add(this.lblTotalNeto);
            this.cardTotales.Controls.Add(this.lblProcesados);
            this.cardTotales.Controls.Add(this.lblTotalIngresos);
            this.cardTotales.Controls.Add(this.lblTotalDeducciones);
            this.cardTotales.Controls.Add(this.btnConfirmarPago);
            this.cardTotales.Controls.Add(this.btnExportarExcel);
            this.cardTotales.Controls.Add(this.btnExportarPdf);
            this.cardTotales.Location = new System.Drawing.Point(392, 96);
            this.cardTotales.Name = "cardTotales";
            this.cardTotales.Size = new System.Drawing.Size(592, 250);
            this.cardTotales.TabIndex = 2;
            // 
            // lblTotalNeto
            // 
            this.lblTotalNeto.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTotalNeto.ForeColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.lblTotalNeto.Location = new System.Drawing.Point(24, 54);
            this.lblTotalNeto.Name = "lblTotalNeto";
            this.lblTotalNeto.Size = new System.Drawing.Size(280, 48);
            this.lblTotalNeto.TabIndex = 0;
            this.lblTotalNeto.Text = "B/. 0.00";
            // 
            // lblProcesados
            // 
            this.lblProcesados.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblProcesados.Location = new System.Drawing.Point(28, 104);
            this.lblProcesados.Name = "lblProcesados";
            this.lblProcesados.Size = new System.Drawing.Size(250, 24);
            this.lblProcesados.TabIndex = 1;
            this.lblProcesados.Text = "0 empleados procesados";
            // 
            // lblTotalIngresos
            // 
            this.lblTotalIngresos.ForeColor = System.Drawing.Color.FromArgb(52, 211, 153);
            this.lblTotalIngresos.Location = new System.Drawing.Point(28, 146);
            this.lblTotalIngresos.Name = "lblTotalIngresos";
            this.lblTotalIngresos.Size = new System.Drawing.Size(230, 26);
            this.lblTotalIngresos.TabIndex = 2;
            this.lblTotalIngresos.Text = "Ingresos: B/. 0.00";
            // 
            // lblTotalDeducciones
            // 
            this.lblTotalDeducciones.ForeColor = System.Drawing.Color.FromArgb(249, 115, 134);
            this.lblTotalDeducciones.Location = new System.Drawing.Point(28, 180);
            this.lblTotalDeducciones.Name = "lblTotalDeducciones";
            this.lblTotalDeducciones.Size = new System.Drawing.Size(250, 26);
            this.lblTotalDeducciones.TabIndex = 3;
            this.lblTotalDeducciones.Text = "Deducciones: B/. 0.00";
            this.lblResumen.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblResumen.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblResumen.Location = new System.Drawing.Point(24, 20);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(260, 26);
            this.lblResumen.TabIndex = 7;
            this.lblResumen.Text = "Resumen del cálculo";
            // 
            // btnConfirmarPago
            // 
            this.btnConfirmarPago.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnConfirmarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarPago.Location = new System.Drawing.Point(390, 34);
            this.btnConfirmarPago.Name = "btnConfirmarPago";
            this.btnConfirmarPago.Size = new System.Drawing.Size(150, 36);
            this.btnConfirmarPago.TabIndex = 4;
            this.btnConfirmarPago.Text = "Confirmar Pago";
            this.btnConfirmarPago.UseVisualStyleBackColor = false;
            this.btnConfirmarPago.Click += new System.EventHandler(this.btnConfirmarPago_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.Black;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarExcel.Location = new System.Drawing.Point(390, 96);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(80, 32);
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
            this.btnExportarPdf.Location = new System.Drawing.Point(480, 96);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(60, 32);
            this.btnExportarPdf.TabIndex = 6;
            this.btnExportarPdf.Text = "PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = false;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // dgvNominaDetalle
            // 
            this.dgvNominaDetalle.AllowUserToAddRows = false;
            this.dgvNominaDetalle.AllowUserToDeleteRows = false;
            this.dgvNominaDetalle.BackgroundColor = System.Drawing.Color.Black;
            this.dgvNominaDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNominaDetalle.Location = new System.Drawing.Point(36, 382);
            this.dgvNominaDetalle.Name = "dgvNominaDetalle";
            this.dgvNominaDetalle.ReadOnly = true;
            this.dgvNominaDetalle.RowHeadersVisible = false;
            this.dgvNominaDetalle.Size = new System.Drawing.Size(948, 318);
            this.dgvNominaDetalle.TabIndex = 3;
            this.dgvNominaDetalle.Columns.Add("Codigo", "Código");
            this.dgvNominaDetalle.Columns.Add("Empleado", "Empleado");
            this.dgvNominaDetalle.Columns.Add("SueldoBase", "Sueldo base");
            this.dgvNominaDetalle.Columns.Add("Bonos", "Bonos");
            this.dgvNominaDetalle.Columns.Add("HorasExtra", "Horas extra");
            this.dgvNominaDetalle.Columns.Add("Deducciones", "Deducciones");
            this.dgvNominaDetalle.Columns.Add("Neto", "Neto a pagar");
            // 
            // FrmNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.dgvNominaDetalle);
            this.Controls.Add(this.cardTotales);
            this.Controls.Add(this.panelParametros);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmNomina";
            this.Text = "Nómina";
            this.Load += new System.EventHandler(this.FrmNomina_Load);
            this.panelParametros.ResumeLayout(false);
            this.cardTotales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNominaDetalle)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
