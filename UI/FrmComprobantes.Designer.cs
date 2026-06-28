namespace SistemaGestionNomina.UI
{
    partial class FrmComprobantes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvComprobantes;
        private System.Windows.Forms.Panel panelVistaPrevia;
        private System.Windows.Forms.Label lblVistaPrevia;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnEnviarEmail;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvComprobantes = new System.Windows.Forms.DataGridView();
            this.panelVistaPrevia = new System.Windows.Forms.Panel();
            this.lblVistaPrevia = new System.Windows.Forms.Label();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnEnviarEmail = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).BeginInit();
            this.panelVistaPrevia.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblTitulo.Location = new System.Drawing.Point(34, 24);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(420, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Comprobantes de pago";
            // 
            // txtBuscar
            // 
            this.txtBuscar.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtBuscar.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtBuscar.Location = new System.Drawing.Point(36, 92);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(320, 23);
            this.txtBuscar.TabIndex = 1;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // dgvComprobantes
            // 
            this.dgvComprobantes.AllowUserToAddRows = false;
            this.dgvComprobantes.AllowUserToDeleteRows = false;
            this.dgvComprobantes.BackgroundColor = System.Drawing.Color.Black;
            this.dgvComprobantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComprobantes.Location = new System.Drawing.Point(36, 144);
            this.dgvComprobantes.Name = "dgvComprobantes";
            this.dgvComprobantes.ReadOnly = true;
            this.dgvComprobantes.RowHeadersVisible = false;
            this.dgvComprobantes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComprobantes.Size = new System.Drawing.Size(480, 460);
            this.dgvComprobantes.TabIndex = 2;
            this.dgvComprobantes.Columns.Add("IdComprobante", "Id");
            this.dgvComprobantes.Columns["IdComprobante"].Visible = false;
            this.dgvComprobantes.Columns.Add("Numero", "Número");
            this.dgvComprobantes.Columns.Add("Empleado", "Empleado");
            this.dgvComprobantes.Columns.Add("Periodo", "Periodo");
            this.dgvComprobantes.Columns.Add("Neto", "Neto");
            this.dgvComprobantes.SelectionChanged += new System.EventHandler(this.dgvComprobantes_SelectionChanged);
            // 
            // panelVistaPrevia
            // 
            this.panelVistaPrevia.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.panelVistaPrevia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVistaPrevia.Controls.Add(this.lblVistaPrevia);
            this.panelVistaPrevia.Location = new System.Drawing.Point(548, 144);
            this.panelVistaPrevia.Name = "panelVistaPrevia";
            this.panelVistaPrevia.Size = new System.Drawing.Size(436, 460);
            this.panelVistaPrevia.TabIndex = 3;
            // 
            // lblVistaPrevia
            // 
            this.lblVistaPrevia.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblVistaPrevia.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblVistaPrevia.Location = new System.Drawing.Point(22, 22);
            this.lblVistaPrevia.Name = "lblVistaPrevia";
            this.lblVistaPrevia.Size = new System.Drawing.Size(390, 410);
            this.lblVistaPrevia.TabIndex = 0;
            this.lblVistaPrevia.Text = "Seleccione un comprobante para ver el detalle.";
            // 
            // buttons
            // 
            this.btnExportarPdf.BackColor = System.Drawing.Color.Black;
            this.btnExportarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPdf.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarPdf.Location = new System.Drawing.Point(568, 86);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(100, 34);
            this.btnExportarPdf.TabIndex = 4;
            this.btnExportarPdf.Text = "PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = false;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            this.btnExportarExcel.BackColor = System.Drawing.Color.Black;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarExcel.Location = new System.Drawing.Point(678, 86);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(110, 34);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            this.btnImprimir.BackColor = System.Drawing.Color.Black;
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnImprimir.Location = new System.Drawing.Point(798, 86);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(80, 34);
            this.btnImprimir.TabIndex = 6;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            this.btnEnviarEmail.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnEnviarEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarEmail.Location = new System.Drawing.Point(888, 86);
            this.btnEnviarEmail.Name = "btnEnviarEmail";
            this.btnEnviarEmail.Size = new System.Drawing.Size(96, 34);
            this.btnEnviarEmail.TabIndex = 7;
            this.btnEnviarEmail.Text = "Enviar Email";
            this.btnEnviarEmail.UseVisualStyleBackColor = false;
            this.btnEnviarEmail.Click += new System.EventHandler(this.btnEnviarEmail_Click);
            // 
            // FrmComprobantes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.btnEnviarEmail);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.panelVistaPrevia);
            this.Controls.Add(this.dgvComprobantes);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmComprobantes";
            this.Text = "Comprobantes";
            this.Load += new System.EventHandler(this.FrmComprobantes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).EndInit();
            this.panelVistaPrevia.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
