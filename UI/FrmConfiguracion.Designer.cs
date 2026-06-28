namespace SistemaGestionNomina.UI
{
    partial class FrmConfiguracion
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelConfiguracion;
        private System.Windows.Forms.NumericUpDown nudSeguroSocial;
        private System.Windows.Forms.NumericUpDown nudISR;
        private System.Windows.Forms.NumericUpDown nudSeguroEducativo;
        private System.Windows.Forms.NumericUpDown nudRecargoHoraExtra;
        private System.Windows.Forms.NumericUpDown nudHorasMensualesBase;
        private System.Windows.Forms.Label lblSeguroSocial;
        private System.Windows.Forms.Label lblISR;
        private System.Windows.Forms.Label lblSeguroEducativo;
        private System.Windows.Forms.Label lblRecargoHoraExtra;
        private System.Windows.Forms.Label lblHorasMensualesBase;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnDescartar;
        private System.Windows.Forms.Button btnBackup;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelConfiguracion = new System.Windows.Forms.Panel();
            this.nudSeguroSocial = new System.Windows.Forms.NumericUpDown();
            this.nudISR = new System.Windows.Forms.NumericUpDown();
            this.nudSeguroEducativo = new System.Windows.Forms.NumericUpDown();
            this.nudRecargoHoraExtra = new System.Windows.Forms.NumericUpDown();
            this.nudHorasMensualesBase = new System.Windows.Forms.NumericUpDown();
            this.lblSeguroSocial = new System.Windows.Forms.Label();
            this.lblISR = new System.Windows.Forms.Label();
            this.lblSeguroEducativo = new System.Windows.Forms.Label();
            this.lblRecargoHoraExtra = new System.Windows.Forms.Label();
            this.lblHorasMensualesBase = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnDescartar = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.panelConfiguracion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeguroSocial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudISR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeguroEducativo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecargoHoraExtra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorasMensualesBase)).BeginInit();
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
            this.lblTitulo.Text = "Configuración del Sistema";
            // 
            // panelConfiguracion
            // 
            this.panelConfiguracion.BackColor = System.Drawing.Color.Black;
            this.panelConfiguracion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelConfiguracion.Controls.Add(this.lblSeguroSocial);
            this.panelConfiguracion.Controls.Add(this.lblISR);
            this.panelConfiguracion.Controls.Add(this.lblSeguroEducativo);
            this.panelConfiguracion.Controls.Add(this.lblRecargoHoraExtra);
            this.panelConfiguracion.Controls.Add(this.lblHorasMensualesBase);
            this.panelConfiguracion.Controls.Add(this.lblDescripcion);
            this.panelConfiguracion.Controls.Add(this.nudSeguroSocial);
            this.panelConfiguracion.Controls.Add(this.nudISR);
            this.panelConfiguracion.Controls.Add(this.nudSeguroEducativo);
            this.panelConfiguracion.Controls.Add(this.nudRecargoHoraExtra);
            this.panelConfiguracion.Controls.Add(this.nudHorasMensualesBase);
            this.panelConfiguracion.Controls.Add(this.btnGuardar);
            this.panelConfiguracion.Controls.Add(this.btnDescartar);
            this.panelConfiguracion.Controls.Add(this.btnBackup);
            this.panelConfiguracion.Location = new System.Drawing.Point(210, 116);
            this.panelConfiguracion.Name = "panelConfiguracion";
            this.panelConfiguracion.Size = new System.Drawing.Size(600, 390);
            this.panelConfiguracion.TabIndex = 1;
            // 
            // numeric controls
            // 
            this.nudSeguroSocial.DecimalPlaces = 2;
            this.nudSeguroSocial.Location = new System.Drawing.Point(42, 54);
            this.nudSeguroSocial.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudSeguroSocial.Name = "nudSeguroSocial";
            this.nudSeguroSocial.Size = new System.Drawing.Size(210, 23);
            this.nudSeguroSocial.TabIndex = 0;
            this.nudISR.DecimalPlaces = 2;
            this.nudISR.Location = new System.Drawing.Point(320, 54);
            this.nudISR.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudISR.Name = "nudISR";
            this.nudISR.Size = new System.Drawing.Size(210, 23);
            this.nudISR.TabIndex = 1;
            this.nudSeguroEducativo.DecimalPlaces = 2;
            this.nudSeguroEducativo.Location = new System.Drawing.Point(42, 132);
            this.nudSeguroEducativo.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudSeguroEducativo.Name = "nudSeguroEducativo";
            this.nudSeguroEducativo.Size = new System.Drawing.Size(210, 23);
            this.nudSeguroEducativo.TabIndex = 2;
            this.nudRecargoHoraExtra.DecimalPlaces = 2;
            this.nudRecargoHoraExtra.Location = new System.Drawing.Point(320, 132);
            this.nudRecargoHoraExtra.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudRecargoHoraExtra.Name = "nudRecargoHoraExtra";
            this.nudRecargoHoraExtra.Size = new System.Drawing.Size(210, 23);
            this.nudRecargoHoraExtra.TabIndex = 3;
            this.nudHorasMensualesBase.Location = new System.Drawing.Point(42, 210);
            this.nudHorasMensualesBase.Maximum = new decimal(new int[] { 400, 0, 0, 0 });
            this.nudHorasMensualesBase.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudHorasMensualesBase.Name = "nudHorasMensualesBase";
            this.nudHorasMensualesBase.Size = new System.Drawing.Size(210, 23);
            this.nudHorasMensualesBase.TabIndex = 4;
            this.nudHorasMensualesBase.Value = new decimal(new int[] { 160, 0, 0, 0 });
            // 
            // labels
            // 
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblDescripcion.Location = new System.Drawing.Point(42, 18);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(500, 24);
            this.lblDescripcion.TabIndex = 7;
            this.lblDescripcion.Text = "Valores académicos configurables para el cálculo de nómina.";
            this.lblSeguroSocial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSeguroSocial.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblSeguroSocial.Location = new System.Drawing.Point(42, 34);
            this.lblSeguroSocial.Name = "lblSeguroSocial";
            this.lblSeguroSocial.Size = new System.Drawing.Size(210, 18);
            this.lblSeguroSocial.TabIndex = 8;
            this.lblSeguroSocial.Text = "Seguro Social (%)";
            this.lblISR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblISR.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblISR.Location = new System.Drawing.Point(320, 34);
            this.lblISR.Name = "lblISR";
            this.lblISR.Size = new System.Drawing.Size(210, 18);
            this.lblISR.TabIndex = 9;
            this.lblISR.Text = "ISR (%)";
            this.lblSeguroEducativo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSeguroEducativo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblSeguroEducativo.Location = new System.Drawing.Point(42, 112);
            this.lblSeguroEducativo.Name = "lblSeguroEducativo";
            this.lblSeguroEducativo.Size = new System.Drawing.Size(210, 18);
            this.lblSeguroEducativo.TabIndex = 10;
            this.lblSeguroEducativo.Text = "Seguro Educativo (%)";
            this.lblRecargoHoraExtra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecargoHoraExtra.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblRecargoHoraExtra.Location = new System.Drawing.Point(320, 112);
            this.lblRecargoHoraExtra.Name = "lblRecargoHoraExtra";
            this.lblRecargoHoraExtra.Size = new System.Drawing.Size(210, 18);
            this.lblRecargoHoraExtra.TabIndex = 11;
            this.lblRecargoHoraExtra.Text = "Recargo de horas extra";
            this.lblHorasMensualesBase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHorasMensualesBase.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblHorasMensualesBase.Location = new System.Drawing.Point(42, 190);
            this.lblHorasMensualesBase.Name = "lblHorasMensualesBase";
            this.lblHorasMensualesBase.Size = new System.Drawing.Size(210, 18);
            this.lblHorasMensualesBase.TabIndex = 12;
            this.lblHorasMensualesBase.Text = "Horas mensuales base";
            // 
            // buttons
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Location = new System.Drawing.Point(320, 304);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(130, 34);
            this.btnGuardar.TabIndex = 5;
            this.btnGuardar.Text = "Guardar cambios";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnDescartar.BackColor = System.Drawing.Color.Black;
            this.btnDescartar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescartar.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnDescartar.Location = new System.Drawing.Point(462, 304);
            this.btnDescartar.Name = "btnDescartar";
            this.btnDescartar.Size = new System.Drawing.Size(92, 34);
            this.btnDescartar.TabIndex = 6;
            this.btnDescartar.Text = "Descartar";
            this.btnDescartar.UseVisualStyleBackColor = false;
            this.btnDescartar.Click += new System.EventHandler(this.btnDescartar_Click);
            this.btnBackup.BackColor = System.Drawing.Color.Black;
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnBackup.Location = new System.Drawing.Point(42, 304);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(150, 34);
            this.btnBackup.TabIndex = 7;
            this.btnBackup.Text = "Crear backup";
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // FrmConfiguracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.panelConfiguracion);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmConfiguracion";
            this.Text = "Configuración";
            this.Load += new System.EventHandler(this.FrmConfiguracion_Load);
            this.panelConfiguracion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSeguroSocial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudISR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeguroEducativo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecargoHoraExtra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorasMensualesBase)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
