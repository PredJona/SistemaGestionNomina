namespace SistemaGestionNomina.UI
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Button btnRegistrarEmpleado;
        private System.Windows.Forms.Button btnProcesarNomina;
        private System.Windows.Forms.Panel cardEmpleados;
        private System.Windows.Forms.Panel cardNomina;
        private System.Windows.Forms.Panel cardComprobantes;
        private System.Windows.Forms.Panel cardAlertas;
        private System.Windows.Forms.Label lblEmpleadosActivosValor;
        private System.Windows.Forms.Label lblNominaActualValor;
        private System.Windows.Forms.Label lblComprobantesValor;
        private System.Windows.Forms.Label lblAlertasValor;
        private System.Windows.Forms.Label lblCardEmpleados;
        private System.Windows.Forms.Label lblCardNomina;
        private System.Windows.Forms.Label lblCardComprobantes;
        private System.Windows.Forms.Label lblCardAlertas;
        private System.Windows.Forms.Label lblActividadTitulo;
        private System.Windows.Forms.Label lblActividadUno;
        private System.Windows.Forms.Label lblActividadDos;
        private System.Windows.Forms.Label lblActividadTres;
        private System.Windows.Forms.Panel panelGrafico;
        private System.Windows.Forms.Panel panelActividad;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPagos;

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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.btnRegistrarEmpleado = new System.Windows.Forms.Button();
            this.btnProcesarNomina = new System.Windows.Forms.Button();
            this.cardEmpleados = new System.Windows.Forms.Panel();
            this.lblCardEmpleados = new System.Windows.Forms.Label();
            this.lblEmpleadosActivosValor = new System.Windows.Forms.Label();
            this.cardNomina = new System.Windows.Forms.Panel();
            this.lblCardNomina = new System.Windows.Forms.Label();
            this.lblNominaActualValor = new System.Windows.Forms.Label();
            this.cardComprobantes = new System.Windows.Forms.Panel();
            this.lblCardComprobantes = new System.Windows.Forms.Label();
            this.lblComprobantesValor = new System.Windows.Forms.Label();
            this.cardAlertas = new System.Windows.Forms.Panel();
            this.lblCardAlertas = new System.Windows.Forms.Label();
            this.lblAlertasValor = new System.Windows.Forms.Label();
            this.panelGrafico = new System.Windows.Forms.Panel();
            this.chartPagos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelActividad = new System.Windows.Forms.Panel();
            this.lblActividadTitulo = new System.Windows.Forms.Label();
            this.lblActividadUno = new System.Windows.Forms.Label();
            this.lblActividadDos = new System.Windows.Forms.Label();
            this.lblActividadTres = new System.Windows.Forms.Label();
            this.cardEmpleados.SuspendLayout();
            this.cardNomina.SuspendLayout();
            this.cardComprobantes.SuspendLayout();
            this.cardAlertas.SuspendLayout();
            this.panelGrafico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPagos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblTitulo.Location = new System.Drawing.Point(34, 28);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(420, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Panel Ejecutivo";
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblSubtitulo.Location = new System.Drawing.Point(38, 70);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(520, 25);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Resumen ejecutivo del periodo actual de nómina.";
            // 
            // btnRegistrarEmpleado
            // 
            this.btnRegistrarEmpleado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistrarEmpleado.BackColor = System.Drawing.Color.Black;
            this.btnRegistrarEmpleado.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            this.btnRegistrarEmpleado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrarEmpleado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarEmpleado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.btnRegistrarEmpleado.Location = new System.Drawing.Point(650, 42);
            this.btnRegistrarEmpleado.Name = "btnRegistrarEmpleado";
            this.btnRegistrarEmpleado.Size = new System.Drawing.Size(170, 38);
            this.btnRegistrarEmpleado.TabIndex = 2;
            this.btnRegistrarEmpleado.Text = "+ Nuevo empleado";
            this.btnRegistrarEmpleado.UseVisualStyleBackColor = false;
            this.btnRegistrarEmpleado.Click += new System.EventHandler(this.btnRegistrarEmpleado_Click);
            // 
            // btnProcesarNomina
            // 
            this.btnProcesarNomina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcesarNomina.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(139)))), ((int)(((byte)(250)))));
            this.btnProcesarNomina.FlatAppearance.BorderSize = 0;
            this.btnProcesarNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcesarNomina.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnProcesarNomina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(25)))), ((int)(((byte)(92)))));
            this.btnProcesarNomina.Location = new System.Drawing.Point(836, 42);
            this.btnProcesarNomina.Name = "btnProcesarNomina";
            this.btnProcesarNomina.Size = new System.Drawing.Size(170, 38);
            this.btnProcesarNomina.TabIndex = 3;
            this.btnProcesarNomina.Text = "Procesar nómina";
            this.btnProcesarNomina.UseVisualStyleBackColor = false;
            this.btnProcesarNomina.Click += new System.EventHandler(this.btnProcesarNomina_Click);
            // 
            // cardEmpleados
            // 
            this.cardEmpleados.BackColor = System.Drawing.Color.Black;
            this.cardEmpleados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardEmpleados.Controls.Add(this.lblCardEmpleados);
            this.cardEmpleados.Controls.Add(this.lblEmpleadosActivosValor);
            this.cardEmpleados.Location = new System.Drawing.Point(36, 120);
            this.cardEmpleados.Name = "cardEmpleados";
            this.cardEmpleados.Size = new System.Drawing.Size(230, 130);
            this.cardEmpleados.TabIndex = 4;
            // 
            // lblCardEmpleados
            // 
            this.lblCardEmpleados.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardEmpleados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCardEmpleados.Location = new System.Drawing.Point(18, 18);
            this.lblCardEmpleados.Name = "lblCardEmpleados";
            this.lblCardEmpleados.Size = new System.Drawing.Size(180, 24);
            this.lblCardEmpleados.TabIndex = 0;
            this.lblCardEmpleados.Text = "Empleados activos";
            // 
            // lblEmpleadosActivosValor
            // 
            this.lblEmpleadosActivosValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblEmpleadosActivosValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblEmpleadosActivosValor.Location = new System.Drawing.Point(18, 56);
            this.lblEmpleadosActivosValor.Name = "lblEmpleadosActivosValor";
            this.lblEmpleadosActivosValor.Size = new System.Drawing.Size(170, 44);
            this.lblEmpleadosActivosValor.TabIndex = 1;
            this.lblEmpleadosActivosValor.Text = "0";
            // 
            // cardNomina
            // 
            this.cardNomina.BackColor = System.Drawing.Color.Black;
            this.cardNomina.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNomina.Controls.Add(this.lblCardNomina);
            this.cardNomina.Controls.Add(this.lblNominaActualValor);
            this.cardNomina.Location = new System.Drawing.Point(282, 120);
            this.cardNomina.Name = "cardNomina";
            this.cardNomina.Size = new System.Drawing.Size(230, 130);
            this.cardNomina.TabIndex = 5;
            // 
            // lblCardNomina
            // 
            this.lblCardNomina.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardNomina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCardNomina.Location = new System.Drawing.Point(18, 18);
            this.lblCardNomina.Name = "lblCardNomina";
            this.lblCardNomina.Size = new System.Drawing.Size(180, 24);
            this.lblCardNomina.TabIndex = 0;
            this.lblCardNomina.Text = "Nómina actual";
            // 
            // lblNominaActualValor
            // 
            this.lblNominaActualValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblNominaActualValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblNominaActualValor.Location = new System.Drawing.Point(18, 56);
            this.lblNominaActualValor.Name = "lblNominaActualValor";
            this.lblNominaActualValor.Size = new System.Drawing.Size(200, 44);
            this.lblNominaActualValor.TabIndex = 1;
            this.lblNominaActualValor.Text = "B/. 0.00";
            // 
            // cardComprobantes
            // 
            this.cardComprobantes.BackColor = System.Drawing.Color.Black;
            this.cardComprobantes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardComprobantes.Controls.Add(this.lblCardComprobantes);
            this.cardComprobantes.Controls.Add(this.lblComprobantesValor);
            this.cardComprobantes.Location = new System.Drawing.Point(528, 120);
            this.cardComprobantes.Name = "cardComprobantes";
            this.cardComprobantes.Size = new System.Drawing.Size(230, 130);
            this.cardComprobantes.TabIndex = 6;
            // 
            // lblCardComprobantes
            // 
            this.lblCardComprobantes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardComprobantes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCardComprobantes.Location = new System.Drawing.Point(18, 18);
            this.lblCardComprobantes.Name = "lblCardComprobantes";
            this.lblCardComprobantes.Size = new System.Drawing.Size(190, 24);
            this.lblCardComprobantes.TabIndex = 0;
            this.lblCardComprobantes.Text = "Comprobantes generados";
            // 
            // lblComprobantesValor
            // 
            this.lblComprobantesValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblComprobantesValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(92)))), ((int)(((byte)(246)))));
            this.lblComprobantesValor.Location = new System.Drawing.Point(18, 56);
            this.lblComprobantesValor.Name = "lblComprobantesValor";
            this.lblComprobantesValor.Size = new System.Drawing.Size(170, 44);
            this.lblComprobantesValor.TabIndex = 1;
            this.lblComprobantesValor.Text = "0";
            // 
            // cardAlertas
            // 
            this.cardAlertas.BackColor = System.Drawing.Color.Black;
            this.cardAlertas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAlertas.Controls.Add(this.lblCardAlertas);
            this.cardAlertas.Controls.Add(this.lblAlertasValor);
            this.cardAlertas.Location = new System.Drawing.Point(774, 120);
            this.cardAlertas.Name = "cardAlertas";
            this.cardAlertas.Size = new System.Drawing.Size(230, 130);
            this.cardAlertas.TabIndex = 7;
            // 
            // lblCardAlertas
            // 
            this.lblCardAlertas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardAlertas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            this.lblCardAlertas.Location = new System.Drawing.Point(18, 18);
            this.lblCardAlertas.Name = "lblCardAlertas";
            this.lblCardAlertas.Size = new System.Drawing.Size(180, 24);
            this.lblCardAlertas.TabIndex = 0;
            this.lblCardAlertas.Text = "Deducciones del mes";
            // 
            // lblAlertasValor
            // 
            this.lblAlertasValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblAlertasValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(134)))));
            this.lblAlertasValor.Location = new System.Drawing.Point(18, 56);
            this.lblAlertasValor.Name = "lblAlertasValor";
            this.lblAlertasValor.Size = new System.Drawing.Size(200, 44);
            this.lblAlertasValor.TabIndex = 1;
            this.lblAlertasValor.Text = "0";
            // 
            // panelGrafico
            // 
            this.panelGrafico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.panelGrafico.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGrafico.Controls.Add(this.chartPagos);
            this.panelGrafico.Location = new System.Drawing.Point(36, 290);
            this.panelGrafico.Name = "panelGrafico";
            this.panelGrafico.Size = new System.Drawing.Size(650, 330);
            this.panelGrafico.TabIndex = 8;
            // 
            // chartPagos
            // 
            this.chartPagos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(185)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            chartArea1.Name = "Pagos";
            this.chartPagos.ChartAreas.Add(chartArea1);
            this.chartPagos.Location = new System.Drawing.Point(20, 42);
            this.chartPagos.Name = "chartPagos";
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(139)))), ((int)(((byte)(250)))));
            series1.BorderWidth = 3;
            series1.ChartArea = "Pagos";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(167)))), ((int)(((byte)(139)))), ((int)(((byte)(250)))));
            series1.Name = "Neto";
            this.chartPagos.Series.Add(series1);
            this.chartPagos.Size = new System.Drawing.Size(600, 260);
            this.chartPagos.TabIndex = 0;
            this.chartPagos.Text = "Tendencia de pagos";
            // 
            // panelActividad
            // 
            this.panelActividad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(34)))));
            this.panelActividad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelActividad.Controls.Add(this.lblActividadTitulo);
            this.panelActividad.Controls.Add(this.lblActividadUno);
            this.panelActividad.Controls.Add(this.lblActividadDos);
            this.panelActividad.Controls.Add(this.lblActividadTres);
            this.panelActividad.Location = new System.Drawing.Point(710, 290);
            this.panelActividad.Name = "panelActividad";
            this.panelActividad.Size = new System.Drawing.Size(294, 330);
            this.panelActividad.TabIndex = 9;
            // 
            // lblActividadTitulo
            // 
            this.lblActividadTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblActividadTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblActividadTitulo.Location = new System.Drawing.Point(20, 22);
            this.lblActividadTitulo.Name = "lblActividadTitulo";
            this.lblActividadTitulo.Size = new System.Drawing.Size(240, 30);
            this.lblActividadTitulo.TabIndex = 0;
            this.lblActividadTitulo.Text = "Actividad reciente";
            // 
            // lblActividadUno
            // 
            this.lblActividadUno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblActividadUno.Location = new System.Drawing.Point(24, 82);
            this.lblActividadUno.Name = "lblActividadUno";
            this.lblActividadUno.Size = new System.Drawing.Size(230, 42);
            this.lblActividadUno.TabIndex = 1;
            this.lblActividadUno.Text = "Nómina lista para revisión\r\nHace 2 horas";
            // 
            // lblActividadDos
            // 
            this.lblActividadDos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.lblActividadDos.Location = new System.Drawing.Point(24, 146);
            this.lblActividadDos.Name = "lblActividadDos";
            this.lblActividadDos.Size = new System.Drawing.Size(230, 42);
            this.lblActividadDos.TabIndex = 2;
            this.lblActividadDos.Text = "Nuevo empleado registrado\r\nDatos iniciales cargados";
            // 
            // lblActividadTres
            // 
            this.lblActividadTres.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(134)))));
            this.lblActividadTres.Location = new System.Drawing.Point(24, 210);
            this.lblActividadTres.Name = "lblActividadTres";
            this.lblActividadTres.Size = new System.Drawing.Size(230, 42);
            this.lblActividadTres.TabIndex = 3;
            this.lblActividadTres.Text = "Validar asistencia pendiente\r\nRevisar tardanzas";
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(14)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.panelActividad);
            this.Controls.Add(this.panelGrafico);
            this.Controls.Add(this.cardAlertas);
            this.Controls.Add(this.cardComprobantes);
            this.Controls.Add(this.cardNomina);
            this.Controls.Add(this.cardEmpleados);
            this.Controls.Add(this.btnProcesarNomina);
            this.Controls.Add(this.btnRegistrarEmpleado);
            this.Controls.Add(this.lblSubtitulo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.Name = "FrmDashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.cardEmpleados.ResumeLayout(false);
            this.cardNomina.ResumeLayout(false);
            this.cardComprobantes.ResumeLayout(false);
            this.cardAlertas.ResumeLayout(false);
            this.panelGrafico.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPagos)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
