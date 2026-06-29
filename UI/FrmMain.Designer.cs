namespace SistemaGestionNomina.UI
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        public System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelTopbar;
        public System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.Label lblSubmarca;
        private System.Windows.Forms.Label lblTituloTop;
        private System.Windows.Forms.Label lblUsuarioActual;
        public FontAwesome.Sharp.IconButton btnDashboard;
        public FontAwesome.Sharp.IconButton btnEmpleados;
        public FontAwesome.Sharp.IconButton btnAsistencia;
        public FontAwesome.Sharp.IconButton btnNomina;
        public FontAwesome.Sharp.IconButton btnComprobantes;
        public FontAwesome.Sharp.IconButton btnReportes;
        public FontAwesome.Sharp.IconButton btnConfiguracion;
        public FontAwesome.Sharp.IconButton btnAcercaDe;
        public FontAwesome.Sharp.IconButton btnSalir;
        public FontAwesome.Sharp.IconButton btnCerrarSesion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.lblMarca = new System.Windows.Forms.Label();
            this.lblSubmarca = new System.Windows.Forms.Label();
            this.btnDashboard = new FontAwesome.Sharp.IconButton();
            this.btnEmpleados = new FontAwesome.Sharp.IconButton();
            this.btnAsistencia = new FontAwesome.Sharp.IconButton();
            this.btnNomina = new FontAwesome.Sharp.IconButton();
            this.btnComprobantes = new FontAwesome.Sharp.IconButton();
            this.btnReportes = new FontAwesome.Sharp.IconButton();
            this.btnConfiguracion = new FontAwesome.Sharp.IconButton();
            this.btnAcercaDe = new FontAwesome.Sharp.IconButton();
            this.btnSalir = new FontAwesome.Sharp.IconButton();
            this.btnCerrarSesion = new FontAwesome.Sharp.IconButton();
            this.panelTopbar = new System.Windows.Forms.Panel();
            this.lblTituloTop = new System.Windows.Forms.Label();
            this.lblUsuarioActual = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelSidebar.SuspendLayout();
            this.panelTopbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(25)))));
            this.panelSidebar.Controls.Add(this.lblMarca);
            this.panelSidebar.Controls.Add(this.lblSubmarca);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.btnEmpleados);
            this.panelSidebar.Controls.Add(this.btnAsistencia);
            this.panelSidebar.Controls.Add(this.btnNomina);
            this.panelSidebar.Controls.Add(this.btnComprobantes);
            this.panelSidebar.Controls.Add(this.btnReportes);
            this.panelSidebar.Controls.Add(this.btnConfiguracion);
            this.panelSidebar.Controls.Add(this.btnAcercaDe);
            this.panelSidebar.Controls.Add(this.btnSalir);
            this.panelSidebar.Controls.Add(this.btnCerrarSesion);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(260, 820);
            this.panelSidebar.TabIndex = 0;
            // 
            // lblMarca
            // 
            this.lblMarca.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.lblMarca.Location = new System.Drawing.Point(24, 30);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(210, 32);
            this.lblMarca.TabIndex = 0;
            this.lblMarca.Text = "Proy2_Eq01";
            // 
            // lblSubmarca
            // 
            this.lblSubmarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblSubmarca.Location = new System.Drawing.Point(26, 64);
            this.lblSubmarca.Name = "lblSubmarca";
            this.lblSubmarca.Size = new System.Drawing.Size(210, 22);
            this.lblSubmarca.TabIndex = 1;
            this.lblSubmarca.Text = "Sistema de nómina";
            // 
            // btnDashboard
            // 
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.btnDashboard.IconChar = FontAwesome.Sharp.IconChar.BorderAll;
            this.btnDashboard.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(139)))), ((int)(((byte)(250)))));
            this.btnDashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDashboard.IconSize = 20;
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Location = new System.Drawing.Point(16, 128);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(228, 44);
            this.btnDashboard.TabIndex = 2;
            this.btnDashboard.Text = "  Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnEmpleados
            // 
            this.btnEmpleados.FlatAppearance.BorderSize = 0;
            this.btnEmpleados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmpleados.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEmpleados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnEmpleados.IconChar = FontAwesome.Sharp.IconChar.Users;
            this.btnEmpleados.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnEmpleados.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEmpleados.IconSize = 20;
            this.btnEmpleados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmpleados.Location = new System.Drawing.Point(16, 184);
            this.btnEmpleados.Name = "btnEmpleados";
            this.btnEmpleados.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnEmpleados.Size = new System.Drawing.Size(228, 44);
            this.btnEmpleados.TabIndex = 3;
            this.btnEmpleados.Text = "  Empleados";
            this.btnEmpleados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmpleados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEmpleados.UseVisualStyleBackColor = true;
            this.btnEmpleados.Click += new System.EventHandler(this.btnEmpleados_Click);
            // 
            // btnAsistencia
            // 
            this.btnAsistencia.FlatAppearance.BorderSize = 0;
            this.btnAsistencia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAsistencia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAsistencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnAsistencia.IconChar = FontAwesome.Sharp.IconChar.CalendarCheck;
            this.btnAsistencia.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnAsistencia.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAsistencia.IconSize = 20;
            this.btnAsistencia.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAsistencia.Location = new System.Drawing.Point(16, 240);
            this.btnAsistencia.Name = "btnAsistencia";
            this.btnAsistencia.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnAsistencia.Size = new System.Drawing.Size(228, 44);
            this.btnAsistencia.TabIndex = 4;
            this.btnAsistencia.Text = "  Asistencia";
            this.btnAsistencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAsistencia.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAsistencia.UseVisualStyleBackColor = true;
            this.btnAsistencia.Click += new System.EventHandler(this.btnAsistencia_Click);
            // 
            // btnNomina
            // 
            this.btnNomina.FlatAppearance.BorderSize = 0;
            this.btnNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNomina.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNomina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnNomina.IconChar = FontAwesome.Sharp.IconChar.MoneyCheckDollar;
            this.btnNomina.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnNomina.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnNomina.IconSize = 20;
            this.btnNomina.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNomina.Location = new System.Drawing.Point(16, 296);
            this.btnNomina.Name = "btnNomina";
            this.btnNomina.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnNomina.Size = new System.Drawing.Size(228, 44);
            this.btnNomina.TabIndex = 5;
            this.btnNomina.Text = "  Nómina";
            this.btnNomina.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNomina.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNomina.UseVisualStyleBackColor = true;
            this.btnNomina.Click += new System.EventHandler(this.btnNomina_Click);
            // 
            // btnComprobantes
            // 
            this.btnComprobantes.FlatAppearance.BorderSize = 0;
            this.btnComprobantes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComprobantes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnComprobantes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnComprobantes.IconChar = FontAwesome.Sharp.IconChar.Receipt;
            this.btnComprobantes.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnComprobantes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnComprobantes.IconSize = 20;
            this.btnComprobantes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComprobantes.Location = new System.Drawing.Point(16, 352);
            this.btnComprobantes.Name = "btnComprobantes";
            this.btnComprobantes.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnComprobantes.Size = new System.Drawing.Size(228, 44);
            this.btnComprobantes.TabIndex = 6;
            this.btnComprobantes.Text = "  Comprobantes";
            this.btnComprobantes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComprobantes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnComprobantes.UseVisualStyleBackColor = true;
            this.btnComprobantes.Click += new System.EventHandler(this.btnComprobantes_Click);
            // 
            // btnReportes
            // 
            this.btnReportes.FlatAppearance.BorderSize = 0;
            this.btnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnReportes.IconChar = FontAwesome.Sharp.IconChar.ChartSimple;
            this.btnReportes.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnReportes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnReportes.IconSize = 20;
            this.btnReportes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReportes.Location = new System.Drawing.Point(16, 408);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnReportes.Size = new System.Drawing.Size(228, 44);
            this.btnReportes.TabIndex = 7;
            this.btnReportes.Text = "  Reportes";
            this.btnReportes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReportes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReportes.UseVisualStyleBackColor = true;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            // 
            // btnConfiguracion
            // 
            this.btnConfiguracion.FlatAppearance.BorderSize = 0;
            this.btnConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfiguracion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnConfiguracion.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.btnConfiguracion.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnConfiguracion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnConfiguracion.IconSize = 20;
            this.btnConfiguracion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.Location = new System.Drawing.Point(16, 464);
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnConfiguracion.Size = new System.Drawing.Size(228, 44);
            this.btnConfiguracion.TabIndex = 8;
            this.btnConfiguracion.Text = "  Configuración";
            this.btnConfiguracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfiguracion.UseVisualStyleBackColor = true;
            this.btnConfiguracion.Click += new System.EventHandler(this.btnConfiguracion_Click);
            // 
            // btnAcercaDe
            // 
            this.btnAcercaDe.FlatAppearance.BorderSize = 0;
            this.btnAcercaDe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcercaDe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAcercaDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnAcercaDe.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            this.btnAcercaDe.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnAcercaDe.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAcercaDe.IconSize = 20;
            this.btnAcercaDe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAcercaDe.Location = new System.Drawing.Point(16, 520);
            this.btnAcercaDe.Name = "btnAcercaDe";
            this.btnAcercaDe.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnAcercaDe.Size = new System.Drawing.Size(228, 44);
            this.btnAcercaDe.TabIndex = 9;
            this.btnAcercaDe.Text = "  Acerca de";
            this.btnAcercaDe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAcercaDe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAcercaDe.UseVisualStyleBackColor = true;
            this.btnAcercaDe.Click += new System.EventHandler(this.btnAcercaDe_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnSalir.IconChar = FontAwesome.Sharp.IconChar.PowerOff;
            this.btnSalir.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSalir.IconSize = 20;
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(16, 688);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnSalir.Size = new System.Drawing.Size(228, 44);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.Text = "  Salir";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnCerrarSesion.IconChar = FontAwesome.Sharp.IconChar.RightFromBracket;
            this.btnCerrarSesion.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.btnCerrarSesion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrarSesion.IconSize = 20;
            this.btnCerrarSesion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.Location = new System.Drawing.Point(16, 744);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnCerrarSesion.Size = new System.Drawing.Size(228, 44);
            this.btnCerrarSesion.TabIndex = 11;
            this.btnCerrarSesion.Text = "  Cerrar sesión";
            this.btnCerrarSesion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // panelTopbar
            // 
            this.panelTopbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(18)))), ((int)(((byte)(26)))));
            this.panelTopbar.Controls.Add(this.lblTituloTop);
            this.panelTopbar.Controls.Add(this.lblUsuarioActual);
            this.panelTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopbar.Location = new System.Drawing.Point(260, 0);
            this.panelTopbar.Name = "panelTopbar";
            this.panelTopbar.Size = new System.Drawing.Size(1020, 78);
            this.panelTopbar.TabIndex = 1;
            // 
            // lblTituloTop
            // 
            this.lblTituloTop.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTituloTop.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTituloTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.lblTituloTop.Location = new System.Drawing.Point(0, 0);
            this.lblTituloTop.Name = "lblTituloTop";
            this.lblTituloTop.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.lblTituloTop.Size = new System.Drawing.Size(520, 78);
            this.lblTituloTop.TabIndex = 0;
            this.lblTituloTop.Text = "Proy2_Eq01_CamposPD";
            this.lblTituloTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsuarioActual
            // 
            this.lblUsuarioActual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsuarioActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsuarioActual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(170)))));
            this.lblUsuarioActual.Location = new System.Drawing.Point(708, 25);
            this.lblUsuarioActual.Name = "lblUsuarioActual";
            this.lblUsuarioActual.Size = new System.Drawing.Size(282, 24);
            this.lblUsuarioActual.TabIndex = 1;
            this.lblUsuarioActual.Text = "admin | Admin";
            this.lblUsuarioActual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(260, 78);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1020, 742);
            this.panelContent.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1280, 820);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTopbar);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.MinimumSize = new System.Drawing.Size(1120, 720);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proy2_Eq01_CamposPD";
            this.panelSidebar.ResumeLayout(false);
            this.panelTopbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
