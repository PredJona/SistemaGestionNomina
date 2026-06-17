namespace SistemaGestionNomina.UI
{
    partial class FrmAcercaDe
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.Label lblCurso;
        private System.Windows.Forms.Label lblIntegrantes;
        private System.Windows.Forms.Label lblDescripcion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.lblCurso = new System.Windows.Forms.Label();
            this.lblIntegrantes = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.Black;
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.lblTitulo);
            this.panelInfo.Controls.Add(this.lblProyecto);
            this.panelInfo.Controls.Add(this.lblCurso);
            this.panelInfo.Controls.Add(this.lblIntegrantes);
            this.panelInfo.Controls.Add(this.lblDescripcion);
            this.panelInfo.Location = new System.Drawing.Point(120, 108);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(780, 460);
            this.panelInfo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(139)))), ((int)(((byte)(250)))));
            this.lblTitulo.Location = new System.Drawing.Point(34, 28);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(650, 44);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Sistema de Gestión de Nómina";
            // 
            // lblProyecto
            // 
            this.lblProyecto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblProyecto.Location = new System.Drawing.Point(36, 104);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(680, 76);
            this.lblProyecto.TabIndex = 1;
            this.lblProyecto.Text = "Proyecto N° 2\r\nWindows Forms App (.NET Framework 4.8)\r\nVisual Studio 2022 Communi" +
    "ty";
            // 
            // lblCurso
            // 
            this.lblCurso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCurso.Location = new System.Drawing.Point(36, 198);
            this.lblCurso.Name = "lblCurso";
            this.lblCurso.Size = new System.Drawing.Size(680, 64);
            this.lblCurso.TabIndex = 2;
            this.lblCurso.Text = "Universidad Tecnológica de Panamá\r\nFacultad de Ingeniería de Sistemas Computacion" +
    "ales\r\nLenguaje de Programación I";
            // 
            // lblIntegrantes
            // 
            this.lblIntegrantes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblIntegrantes.Location = new System.Drawing.Point(36, 284);
            this.lblIntegrantes.Name = "lblIntegrantes";
            this.lblIntegrantes.Size = new System.Drawing.Size(680, 42);
            this.lblIntegrantes.TabIndex = 3;
            this.lblIntegrantes.Text = "Equipo / Integrantes\r\nJonathan Romero, Jose Martinez, Karen Wen, Davis Batista\r\n";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblDescripcion.Location = new System.Drawing.Point(36, 352);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(680, 72);
            this.lblDescripcion.TabIndex = 4;
            this.lblDescripcion.Text = "Aplicación académica para administrar empleados, asistencia, cálculo básico de nó" +
    "mina, comprobantes y reportes administrativos con separación por capas.";
            // 
            // FrmAcercaDe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.panelInfo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.Name = "FrmAcercaDe";
            this.Text = "Acerca de";
            this.panelInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
