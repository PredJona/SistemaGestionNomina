namespace SistemaGestionNomina.UI
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel cardNomina;
        private System.Windows.Forms.Panel cardEmpleados;
        private System.Windows.Forms.Panel cardPagos;
        private System.Windows.Forms.Panel cardDeducciones;
        private System.Windows.Forms.Label lblCardNomina;
        private System.Windows.Forms.Label lblCardEmpleados;
        private System.Windows.Forms.Label lblCardPagos;
        private System.Windows.Forms.Label lblCardDeducciones;
        private System.Windows.Forms.Button btnReporteNomina;
        private System.Windows.Forms.Button btnReporteEmpleados;
        private System.Windows.Forms.Button btnReportePagos;
        private System.Windows.Forms.Button btnReporteDeducciones;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.DataGridView dgvReportesRecientes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.cardNomina = new System.Windows.Forms.Panel();
            this.lblCardNomina = new System.Windows.Forms.Label();
            this.btnReporteNomina = new System.Windows.Forms.Button();
            this.cardEmpleados = new System.Windows.Forms.Panel();
            this.lblCardEmpleados = new System.Windows.Forms.Label();
            this.btnReporteEmpleados = new System.Windows.Forms.Button();
            this.cardPagos = new System.Windows.Forms.Panel();
            this.lblCardPagos = new System.Windows.Forms.Label();
            this.btnReportePagos = new System.Windows.Forms.Button();
            this.cardDeducciones = new System.Windows.Forms.Panel();
            this.lblCardDeducciones = new System.Windows.Forms.Label();
            this.btnReporteDeducciones = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.dgvReportesRecientes = new System.Windows.Forms.DataGridView();
            this.cardNomina.SuspendLayout();
            this.cardEmpleados.SuspendLayout();
            this.cardPagos.SuspendLayout();
            this.cardDeducciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportesRecientes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblTitulo.Location = new System.Drawing.Point(34, 24);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(520, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes Administrativos";
            // 
            // cards
            // 
            this.cardNomina.BackColor = System.Drawing.Color.Black;
            this.cardNomina.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNomina.Controls.Add(this.lblCardNomina);
            this.cardNomina.Controls.Add(this.btnReporteNomina);
            this.cardNomina.Location = new System.Drawing.Point(36, 100);
            this.cardNomina.Name = "cardNomina";
            this.cardNomina.Size = new System.Drawing.Size(220, 150);
            this.cardNomina.TabIndex = 1;
            this.lblCardNomina.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardNomina.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblCardNomina.Location = new System.Drawing.Point(24, 22);
            this.lblCardNomina.Name = "lblCardNomina";
            this.lblCardNomina.Size = new System.Drawing.Size(170, 46);
            this.lblCardNomina.TabIndex = 1;
            this.lblCardNomina.Text = "Reporte general\r\nde nómina";
            this.btnReporteNomina.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnReporteNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteNomina.Location = new System.Drawing.Point(28, 92);
            this.btnReporteNomina.Name = "btnReporteNomina";
            this.btnReporteNomina.Size = new System.Drawing.Size(160, 34);
            this.btnReporteNomina.TabIndex = 0;
            this.btnReporteNomina.Text = "Reporte Nómina";
            this.btnReporteNomina.UseVisualStyleBackColor = false;
            this.btnReporteNomina.Click += new System.EventHandler(this.btnReporteNomina_Click);
            this.cardEmpleados.BackColor = System.Drawing.Color.Black;
            this.cardEmpleados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardEmpleados.Controls.Add(this.lblCardEmpleados);
            this.cardEmpleados.Controls.Add(this.btnReporteEmpleados);
            this.cardEmpleados.Location = new System.Drawing.Point(278, 100);
            this.cardEmpleados.Name = "cardEmpleados";
            this.cardEmpleados.Size = new System.Drawing.Size(220, 150);
            this.cardEmpleados.TabIndex = 2;
            this.lblCardEmpleados.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardEmpleados.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblCardEmpleados.Location = new System.Drawing.Point(24, 22);
            this.lblCardEmpleados.Name = "lblCardEmpleados";
            this.lblCardEmpleados.Size = new System.Drawing.Size(170, 46);
            this.lblCardEmpleados.TabIndex = 1;
            this.lblCardEmpleados.Text = "Empleados\r\nactivos";
            this.btnReporteEmpleados.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnReporteEmpleados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteEmpleados.Location = new System.Drawing.Point(28, 92);
            this.btnReporteEmpleados.Name = "btnReporteEmpleados";
            this.btnReporteEmpleados.Size = new System.Drawing.Size(160, 34);
            this.btnReporteEmpleados.TabIndex = 0;
            this.btnReporteEmpleados.Text = "Empleados Activos";
            this.btnReporteEmpleados.UseVisualStyleBackColor = false;
            this.btnReporteEmpleados.Click += new System.EventHandler(this.btnReporteEmpleados_Click);
            this.cardPagos.BackColor = System.Drawing.Color.Black;
            this.cardPagos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPagos.Controls.Add(this.lblCardPagos);
            this.cardPagos.Controls.Add(this.btnReportePagos);
            this.cardPagos.Location = new System.Drawing.Point(520, 100);
            this.cardPagos.Name = "cardPagos";
            this.cardPagos.Size = new System.Drawing.Size(220, 150);
            this.cardPagos.TabIndex = 3;
            this.lblCardPagos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardPagos.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblCardPagos.Location = new System.Drawing.Point(24, 22);
            this.lblCardPagos.Name = "lblCardPagos";
            this.lblCardPagos.Size = new System.Drawing.Size(170, 46);
            this.lblCardPagos.TabIndex = 1;
            this.lblCardPagos.Text = "Historial\r\nde pagos";
            this.btnReportePagos.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnReportePagos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportePagos.Location = new System.Drawing.Point(28, 92);
            this.btnReportePagos.Name = "btnReportePagos";
            this.btnReportePagos.Size = new System.Drawing.Size(160, 34);
            this.btnReportePagos.TabIndex = 0;
            this.btnReportePagos.Text = "Historial de Pagos";
            this.btnReportePagos.UseVisualStyleBackColor = false;
            this.btnReportePagos.Click += new System.EventHandler(this.btnReportePagos_Click);
            this.cardDeducciones.BackColor = System.Drawing.Color.Black;
            this.cardDeducciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardDeducciones.Controls.Add(this.lblCardDeducciones);
            this.cardDeducciones.Controls.Add(this.btnReporteDeducciones);
            this.cardDeducciones.Location = new System.Drawing.Point(762, 100);
            this.cardDeducciones.Name = "cardDeducciones";
            this.cardDeducciones.Size = new System.Drawing.Size(220, 150);
            this.cardDeducciones.TabIndex = 4;
            this.lblCardDeducciones.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardDeducciones.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblCardDeducciones.Location = new System.Drawing.Point(24, 22);
            this.lblCardDeducciones.Name = "lblCardDeducciones";
            this.lblCardDeducciones.Size = new System.Drawing.Size(170, 46);
            this.lblCardDeducciones.TabIndex = 1;
            this.lblCardDeducciones.Text = "Resumen de\r\ndeducciones";
            this.btnReporteDeducciones.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnReporteDeducciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteDeducciones.Location = new System.Drawing.Point(28, 92);
            this.btnReporteDeducciones.Name = "btnReporteDeducciones";
            this.btnReporteDeducciones.Size = new System.Drawing.Size(160, 34);
            this.btnReporteDeducciones.TabIndex = 0;
            this.btnReporteDeducciones.Text = "Deducciones";
            this.btnReporteDeducciones.UseVisualStyleBackColor = false;
            this.btnReporteDeducciones.Click += new System.EventHandler(this.btnReporteDeducciones_Click);
            // 
            // export buttons
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.Black;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarExcel.Location = new System.Drawing.Point(776, 284);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(95, 34);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            this.btnExportarPdf.BackColor = System.Drawing.Color.Black;
            this.btnExportarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPdf.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarPdf.Location = new System.Drawing.Point(886, 284);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(95, 34);
            this.btnExportarPdf.TabIndex = 6;
            this.btnExportarPdf.Text = "PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = false;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // dgvReportesRecientes
            // 
            this.dgvReportesRecientes.AllowUserToAddRows = false;
            this.dgvReportesRecientes.AllowUserToDeleteRows = false;
            this.dgvReportesRecientes.BackgroundColor = System.Drawing.Color.Black;
            this.dgvReportesRecientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportesRecientes.Location = new System.Drawing.Point(36, 334);
            this.dgvReportesRecientes.Name = "dgvReportesRecientes";
            this.dgvReportesRecientes.ReadOnly = true;
            this.dgvReportesRecientes.RowHeadersVisible = false;
            this.dgvReportesRecientes.Size = new System.Drawing.Size(946, 330);
            this.dgvReportesRecientes.TabIndex = 7;
            this.dgvReportesRecientes.Columns.Add("Nombre", "Nombre del reporte");
            this.dgvReportesRecientes.Columns.Add("Tipo", "Tipo");
            this.dgvReportesRecientes.Columns.Add("GeneradoPor", "Generado por");
            this.dgvReportesRecientes.Columns.Add("Fecha", "Fecha");
            this.dgvReportesRecientes.Columns.Add("Ruta", "Ruta");
            // 
            // FrmReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.dgvReportesRecientes);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.cardDeducciones);
            this.Controls.Add(this.cardPagos);
            this.Controls.Add(this.cardEmpleados);
            this.Controls.Add(this.cardNomina);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmReportes";
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.FrmReportes_Load);
            this.cardNomina.ResumeLayout(false);
            this.cardEmpleados.ResumeLayout(false);
            this.cardPagos.ResumeLayout(false);
            this.cardDeducciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportesRecientes)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
