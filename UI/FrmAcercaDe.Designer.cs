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
        private System.Windows.Forms.Button btnCerrar;

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
            this.btnCerrar = new System.Windows.Forms.Button();
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
            this.panelInfo.Controls.Add(this.btnCerrar);
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
            this.lblTitulo.Text = "Proy2_Eq01_CamposPD";
            // 
            // lblProyecto
            // 
            this.lblProyecto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblProyecto.Location = new System.Drawing.Point(36, 104);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(680, 90);
            this.lblProyecto.TabIndex = 1;
            this.lblProyecto.Text = "Proyecto N° 2\r\nTema H: Manejo de los campos en las clases + PrintDocument\r\nAplica" +
    "ción: Proy2_Eq01_CamposPD\r\nProblema: control de empleados, a" +
    "sistencia, nómina y comprobantes";
            // 
            // lblCurso
            // 
            this.lblCurso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCurso.Location = new System.Drawing.Point(36, 208);
            this.lblCurso.Name = "lblCurso";
            this.lblCurso.Size = new System.Drawing.Size(680, 82);
            this.lblCurso.TabIndex = 2;
            this.lblCurso.Text = "Universidad Tecnológica de Panamá\r\nFacultad de Ingeniería de Sistemas Computacion" +
    "ales\r\nLenguaje de Programación I\r\nProfesora: Anna Araba de Ruiz | Grupo: 1SF121";
            // 
            // lblIntegrantes
            // 
            this.lblIntegrantes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblIntegrantes.Location = new System.Drawing.Point(36, 304);
            this.lblIntegrantes.Name = "lblIntegrantes";
            this.lblIntegrantes.Size = new System.Drawing.Size(680, 50);
            this.lblIntegrantes.TabIndex = 3;
            this.lblIntegrantes.Text = "Equipo: Eq01\r\nIntegrantes: Jonathan Romero, Jose Martinez, Karen Wen, Davis Batis" +
    "ta";
            this.lblIntegrantes.Click += new System.EventHandler(this.lblIntegrantes_Click);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblDescripcion.Location = new System.Drawing.Point(36, 366);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(520, 72);
            this.lblDescripcion.TabIndex = 4;
            this.lblDescripcion.Text = "Windows Forms App (.NET Framework 4.8)\r\nVisual Studio 2022 | Versión académica 1." +
    "0 | Año 2026\r\nIncluye SQLite, servicios, repositorios, exportación e impresión.";
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(600, 386);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(116, 38);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
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
