using System;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmPortalTrabajador : Form
    {
        private readonly EmployeePortalService portalService = new EmployeePortalService();
        private readonly Action openProfile;
        private readonly Action openAttendance;
        private readonly Action openPayslips;
        private readonly Action openPassword;
        private readonly Action openAbsences;

        public FrmPortalTrabajador() : this(null, null, null, null, null) { }

        public FrmPortalTrabajador(Action profile, Action attendance, Action payslips, Action password,
            Action absences)
        {
            openProfile = profile;
            openAttendance = attendance;
            openPayslips = payslips;
            openPassword = password;
            openAbsences = absences;
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmPortalTrabajador_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            try
            {
                EmployeePortalDashboardViewModel model = portalService.GetDashboard();
                lblBienvenida.Text = "Hola, " + model.Profile.NombreCompleto;
                lblCargoValor.Text = model.Profile.Cargo;
                lblDepartamentoValor.Text = model.Profile.Departamento;
                lblCodigoValor.Text = model.Profile.Codigo;
                lblNetoValor.Text = model.LatestPayslip == null
                    ? "Sin comprobantes"
                    : "B/. " + model.LatestPayslip.NetoPagar.ToString("#,##0.00");
                lblPeriodoValor.Text = model.LatestPayslip == null ? "No disponible" : model.LatestPayslip.PeriodoNombre;
                lblAsistenciaValor.Text = model.AttendanceSummary.Puntuales + " puntuales / " +
                    model.AttendanceSummary.Tardanzas + " tardanzas";

                dgvRecientes.Rows.Clear();
                foreach (Asistencia item in model.RecentAttendance)
                {
                    dgvRecientes.Rows.Add(item.Fecha.ToString("dd/MM/yyyy"), item.Estado,
                        item.HoraEntrada.HasValue ? item.HoraEntrada.Value.ToString(@"hh\:mm") : "-",
                        item.HorasTrabajadas.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Portal del trabajador", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMiPerfil_Click(object sender, EventArgs e) { if (openProfile != null) openProfile(); }
        private void btnMisAsistencias_Click(object sender, EventArgs e) { if (openAttendance != null) openAttendance(); }
        private void btnMisComprobantes_Click(object sender, EventArgs e) { if (openPayslips != null) openPayslips(); }
        private void btnCambiarPassword_Click(object sender, EventArgs e) { if (openPassword != null) openPassword(); }
        private void btnMisSolicitudes_Click(object sender, EventArgs e) { if (openAbsences != null) openAbsences(); }
    }
}
