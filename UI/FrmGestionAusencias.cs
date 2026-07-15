using System;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmGestionAusencias : Form
    {
        private readonly int requestId;
        private readonly bool cancelOnly;
        private readonly AbsenceRequestService service = new AbsenceRequestService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        public bool RequestChanged { get; private set; }

        public FrmGestionAusencias() : this(0, false) { }
        public FrmGestionAusencias(int id, bool showCancelOnly)
        {
            requestId = id; cancelOnly = showCancelOnly; InitializeComponent(); ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmGestionAusencias_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || requestId <= 0) return;
            try
            {
                SolicitudAusencia item = service.GetById(requestId);
                lblSolicitud.Text = item.CodigoEmpleado + " - " + item.EmpleadoNombre;
                lblDetalle.Text = item.Tipo + " | " + item.FechaInicio.ToString("dd/MM/yyyy") + " al " + item.FechaFin.ToString("dd/MM/yyyy") + " | " + item.Estado;
                txtMotivo.Text = item.Motivo;
                btnAprobar.Visible = !cancelOnly && authorizationService.HasPermission(Permissions.AbsencesApprove) && item.Estado == AbsenceStates.Pending;
                btnRechazar.Visible = !cancelOnly && authorizationService.HasPermission(Permissions.AbsencesReject) && item.Estado == AbsenceStates.Pending;
                btnCancelarSolicitud.Visible = (authorizationService.HasPermission(Permissions.AbsencesCancel) || authorizationService.HasPermission(Permissions.OwnAbsencesCancel)) &&
                    (item.Estado == AbsenceStates.Pending || item.Estado == AbsenceStates.Approved);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnAprobar_Click(object sender, EventArgs e) { Execute("aprobar", delegate { service.Approve(requestId, txtObservacion.Text); }); }
        private void btnRechazar_Click(object sender, EventArgs e) { Execute("rechazar", delegate { service.Reject(requestId, txtObservacion.Text); }); }
        private void btnCancelarSolicitud_Click(object sender, EventArgs e) { Execute("cancelar", delegate { service.Cancel(requestId, txtObservacion.Text); }); }
        private void btnCerrar_Click(object sender, EventArgs e) { Close(); }

        private void Execute(string action, Action operation)
        {
            if (MessageBox.Show("¿Confirma que desea " + action + " esta solicitud?", "Ausencias", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try { operation(); RequestChanged = true; DialogResult = DialogResult.OK; Close(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
