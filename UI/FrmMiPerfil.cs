using System;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmMiPerfil : Form
    {
        private readonly EmployeePortalService portalService = new EmployeePortalService();

        public FrmMiPerfil()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmMiPerfil_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            try
            {
                EmployeeProfileViewModel profile = portalService.GetMyProfile();
                lblNombreValor.Text = profile.NombreCompleto;
                lblCodigoValor.Text = profile.Codigo;
                lblCedulaValor.Text = profile.Cedula;
                lblCargoValor.Text = profile.Cargo;
                lblDepartamentoValor.Text = profile.Departamento;
                lblSalarioValor.Text = "B/. " + profile.SalarioBase.ToString("#,##0.00");
                lblEstadoValor.Text = profile.Estado;
                lblIngresoValor.Text = profile.FechaIngreso.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mi perfil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
