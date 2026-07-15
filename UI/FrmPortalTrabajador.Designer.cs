namespace SistemaGestionNomina.UI
{
    partial class FrmPortalTrabajador
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Panel panelPerfil;
        private System.Windows.Forms.Label lblPerfilTitulo;
        private System.Windows.Forms.Label lblCargoValor;
        private System.Windows.Forms.Label lblDepartamentoValor;
        private System.Windows.Forms.Label lblCodigoValor;
        private System.Windows.Forms.Panel panelPago;
        private System.Windows.Forms.Label lblPagoTitulo;
        private System.Windows.Forms.Label lblNetoValor;
        private System.Windows.Forms.Label lblPeriodoValor;
        private System.Windows.Forms.Panel panelAsistencia;
        private System.Windows.Forms.Label lblAsistenciaTitulo;
        private System.Windows.Forms.Label lblAsistenciaValor;
        private System.Windows.Forms.DataGridView dgvRecientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoras;
        private System.Windows.Forms.Button btnMiPerfil;
        private System.Windows.Forms.Button btnMisAsistencias;
        private System.Windows.Forms.Button btnMisComprobantes;
        private System.Windows.Forms.Button btnCambiarPassword;
        private System.Windows.Forms.Button btnMisSolicitudes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.panelPerfil = new System.Windows.Forms.Panel();
            this.lblPerfilTitulo = new System.Windows.Forms.Label();
            this.lblCargoValor = new System.Windows.Forms.Label();
            this.lblDepartamentoValor = new System.Windows.Forms.Label();
            this.lblCodigoValor = new System.Windows.Forms.Label();
            this.panelPago = new System.Windows.Forms.Panel();
            this.lblPagoTitulo = new System.Windows.Forms.Label();
            this.lblNetoValor = new System.Windows.Forms.Label();
            this.lblPeriodoValor = new System.Windows.Forms.Label();
            this.panelAsistencia = new System.Windows.Forms.Panel();
            this.lblAsistenciaTitulo = new System.Windows.Forms.Label();
            this.lblAsistenciaValor = new System.Windows.Forms.Label();
            this.dgvRecientes = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntrada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMiPerfil = new System.Windows.Forms.Button();
            this.btnMisAsistencias = new System.Windows.Forms.Button();
            this.btnMisComprobantes = new System.Windows.Forms.Button();
            this.btnCambiarPassword = new System.Windows.Forms.Button();
            this.btnMisSolicitudes = new System.Windows.Forms.Button();
            this.panelPerfil.SuspendLayout();
            this.panelPago.SuspendLayout();
            this.panelAsistencia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecientes)).BeginInit();
            this.SuspendLayout();
            this.lblBienvenida.AutoSize = true; this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold); this.lblBienvenida.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.lblBienvenida.Location = new System.Drawing.Point(30, 22); this.lblBienvenida.Name = "lblBienvenida"; this.lblBienvenida.Size = new System.Drawing.Size(355, 52); this.lblBienvenida.Text = "Portal del trabajador";
            this.lblSubtitulo.AutoSize = true; this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblSubtitulo.Location = new System.Drawing.Point(34, 76); this.lblSubtitulo.Name = "lblSubtitulo"; this.lblSubtitulo.Size = new System.Drawing.Size(352, 20); this.lblSubtitulo.Text = "Consulta tu informacion laboral y comprobantes.";
            this.panelPerfil.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.panelPerfil.Controls.Add(this.lblPerfilTitulo); this.panelPerfil.Controls.Add(this.lblCargoValor); this.panelPerfil.Controls.Add(this.lblDepartamentoValor); this.panelPerfil.Controls.Add(this.lblCodigoValor); this.panelPerfil.Location = new System.Drawing.Point(34, 116); this.panelPerfil.Name = "panelPerfil"; this.panelPerfil.Size = new System.Drawing.Size(292, 154);
            this.lblPerfilTitulo.AutoSize = true; this.lblPerfilTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold); this.lblPerfilTitulo.Location = new System.Drawing.Point(18, 17); this.lblPerfilTitulo.Text = "Resumen laboral";
            this.lblCargoValor.AutoEllipsis = true; this.lblCargoValor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); this.lblCargoValor.Location = new System.Drawing.Point(18, 56); this.lblCargoValor.Size = new System.Drawing.Size(252, 25); this.lblCargoValor.Text = "Cargo";
            this.lblDepartamentoValor.AutoEllipsis = true; this.lblDepartamentoValor.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblDepartamentoValor.Location = new System.Drawing.Point(18, 86); this.lblDepartamentoValor.Size = new System.Drawing.Size(252, 24); this.lblDepartamentoValor.Text = "Departamento";
            this.lblCodigoValor.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblCodigoValor.Location = new System.Drawing.Point(18, 116); this.lblCodigoValor.Size = new System.Drawing.Size(252, 24); this.lblCodigoValor.Text = "Codigo";
            this.panelPago.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.panelPago.Controls.Add(this.lblPagoTitulo); this.panelPago.Controls.Add(this.lblNetoValor); this.panelPago.Controls.Add(this.lblPeriodoValor); this.panelPago.Location = new System.Drawing.Point(346, 116); this.panelPago.Name = "panelPago"; this.panelPago.Size = new System.Drawing.Size(292, 154);
            this.lblPagoTitulo.AutoSize = true; this.lblPagoTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold); this.lblPagoTitulo.Location = new System.Drawing.Point(18, 17); this.lblPagoTitulo.Text = "Ultimo comprobante";
            this.lblNetoValor.AutoEllipsis = true; this.lblNetoValor.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold); this.lblNetoValor.ForeColor = System.Drawing.Color.FromArgb(52, 211, 153); this.lblNetoValor.Location = new System.Drawing.Point(18, 52); this.lblNetoValor.Size = new System.Drawing.Size(252, 44); this.lblNetoValor.Text = "Sin comprobantes";
            this.lblPeriodoValor.ForeColor = System.Drawing.Color.FromArgb(161, 161, 175); this.lblPeriodoValor.Location = new System.Drawing.Point(18, 111); this.lblPeriodoValor.Size = new System.Drawing.Size(252, 24); this.lblPeriodoValor.Text = "Periodo";
            this.panelAsistencia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))); this.panelAsistencia.BackColor = System.Drawing.Color.FromArgb(24, 25, 34); this.panelAsistencia.Controls.Add(this.lblAsistenciaTitulo); this.panelAsistencia.Controls.Add(this.lblAsistenciaValor); this.panelAsistencia.Location = new System.Drawing.Point(658, 116); this.panelAsistencia.Name = "panelAsistencia"; this.panelAsistencia.Size = new System.Drawing.Size(328, 154);
            this.lblAsistenciaTitulo.AutoSize = true; this.lblAsistenciaTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold); this.lblAsistenciaTitulo.Location = new System.Drawing.Point(18, 17); this.lblAsistenciaTitulo.Text = "Asistencia del mes";
            this.lblAsistenciaValor.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold); this.lblAsistenciaValor.ForeColor = System.Drawing.Color.FromArgb(167, 139, 250); this.lblAsistenciaValor.Location = new System.Drawing.Point(18, 59); this.lblAsistenciaValor.Size = new System.Drawing.Size(286, 62); this.lblAsistenciaValor.Text = "0 puntuales / 0 tardanzas";
            this.btnMiPerfil.Location = new System.Drawing.Point(34, 292); this.btnMiPerfil.Name = "btnMiPerfil"; this.btnMiPerfil.Size = new System.Drawing.Size(176, 42); this.btnMiPerfil.Text = "Mi perfil"; this.btnMiPerfil.Click += new System.EventHandler(this.btnMiPerfil_Click);
            this.btnMisAsistencias.Location = new System.Drawing.Point(228, 292); this.btnMisAsistencias.Name = "btnMisAsistencias"; this.btnMisAsistencias.Size = new System.Drawing.Size(176, 42); this.btnMisAsistencias.Text = "Mis asistencias"; this.btnMisAsistencias.Click += new System.EventHandler(this.btnMisAsistencias_Click);
            this.btnMisComprobantes.Location = new System.Drawing.Point(422, 292); this.btnMisComprobantes.Name = "btnMisComprobantes"; this.btnMisComprobantes.Size = new System.Drawing.Size(176, 42); this.btnMisComprobantes.Text = "Mis comprobantes"; this.btnMisComprobantes.Click += new System.EventHandler(this.btnMisComprobantes_Click);
            this.btnMisSolicitudes.Location = new System.Drawing.Point(616, 292); this.btnMisSolicitudes.Name = "btnMisSolicitudes"; this.btnMisSolicitudes.Size = new System.Drawing.Size(176, 42); this.btnMisSolicitudes.Text = "Mis solicitudes"; this.btnMisSolicitudes.Click += new System.EventHandler(this.btnMisSolicitudes_Click);
            this.btnCambiarPassword.Location = new System.Drawing.Point(810, 292); this.btnCambiarPassword.Name = "btnCambiarPassword"; this.btnCambiarPassword.Size = new System.Drawing.Size(176, 42); this.btnCambiarPassword.Text = "Cambiar contraseña"; this.btnCambiarPassword.Click += new System.EventHandler(this.btnCambiarPassword_Click);
            this.dgvRecientes.AllowUserToAddRows = false; this.dgvRecientes.AllowUserToDeleteRows = false; this.dgvRecientes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))); this.dgvRecientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill; this.dgvRecientes.BackgroundColor = System.Drawing.Color.FromArgb(18, 19, 27); this.dgvRecientes.BorderStyle = System.Windows.Forms.BorderStyle.None; this.dgvRecientes.ColumnHeadersHeight = 40; this.dgvRecientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colFecha, this.colEstado, this.colEntrada, this.colHoras }); this.dgvRecientes.Location = new System.Drawing.Point(34, 362); this.dgvRecientes.Name = "dgvRecientes"; this.dgvRecientes.ReadOnly = true; this.dgvRecientes.RowHeadersVisible = false; this.dgvRecientes.RowTemplate.Height = 36; this.dgvRecientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; this.dgvRecientes.Size = new System.Drawing.Size(952, 340);
            this.colFecha.HeaderText = "Fecha"; this.colFecha.Name = "colFecha"; this.colFecha.ReadOnly = true;
            this.colEstado.HeaderText = "Estado"; this.colEstado.Name = "colEstado"; this.colEstado.ReadOnly = true;
            this.colEntrada.HeaderText = "Entrada"; this.colEntrada.Name = "colEntrada"; this.colEntrada.ReadOnly = true;
            this.colHoras.HeaderText = "Horas"; this.colHoras.Name = "colHoras"; this.colHoras.ReadOnly = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20); this.ClientSize = new System.Drawing.Size(1020, 742); this.Controls.Add(this.dgvRecientes); this.Controls.Add(this.btnCambiarPassword); this.Controls.Add(this.btnMisSolicitudes); this.Controls.Add(this.btnMisComprobantes); this.Controls.Add(this.btnMisAsistencias); this.Controls.Add(this.btnMiPerfil); this.Controls.Add(this.panelAsistencia); this.Controls.Add(this.panelPago); this.Controls.Add(this.panelPerfil); this.Controls.Add(this.lblSubtitulo); this.Controls.Add(this.lblBienvenida); this.Font = new System.Drawing.Font("Segoe UI", 9F); this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245); this.Name = "FrmPortalTrabajador"; this.Text = "Portal del trabajador"; this.Load += new System.EventHandler(this.FrmPortalTrabajador_Load);
            this.panelPerfil.ResumeLayout(false); this.panelPerfil.PerformLayout(); this.panelPago.ResumeLayout(false); this.panelPago.PerformLayout(); this.panelAsistencia.ResumeLayout(false); this.panelAsistencia.PerformLayout(); ((System.ComponentModel.ISupportInitialize)(this.dgvRecientes)).EndInit(); this.ResumeLayout(false); this.PerformLayout();
        }
    }
}
