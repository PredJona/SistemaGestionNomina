using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmDashboard : Form
    {
        private readonly Action openEmployees;
        private readonly Action openPayroll;
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly NominaService nominaService = new NominaService();
        private readonly ComprobanteService comprobanteService = new ComprobanteService();

        public FrmDashboard()
            : this(null, null)
        {
        }

        public FrmDashboard(Action openEmployeesAction, Action openPayrollAction)
        {
            openEmployees = openEmployeesAction;
            openPayroll = openPayrollAction;
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                RefreshDashboard();
            }
        }

        private void btnRegistrarEmpleado_Click(object sender, EventArgs e)
        {
            if (openEmployees != null)
            {
                openEmployees();
            }
        }

        private void btnProcesarNomina_Click(object sender, EventArgs e)
        {
            if (openPayroll != null)
            {
                openPayroll();
            }
        }

        private void RefreshDashboard()
        {
            List<Empleado> activos = empleadoService.GetActive(null);
            List<Nomina> nominas = nominaService.GetNominas();
            List<Comprobante> comprobantes = comprobanteService.GetAll(string.Empty);
            decimal totalNomina = nominas.Count > 0 ? nominas[0].TotalNeto : 0;

            lblEmpleadosActivosValor.Text = activos.Count.ToString();
            lblNominaActualValor.Text = "B/. " + totalNomina.ToString("0.00");
            lblComprobantesValor.Text = comprobantes.Count.ToString();
            lblAlertasValor.Text = "2";

            chartPagos.Series["Neto"].Points.Clear();
            string[] months = { "Ene", "Feb", "Mar", "Abr", "May", "Jun" };
            decimal baseValue = totalNomina > 0 ? totalNomina : 8500;
            for (int i = 0; i < months.Length; i++)
            {
                chartPagos.Series["Neto"].Points.AddXY(months[i], Convert.ToDouble(baseValue + (i * 420)));
            }
        }
    }
}
