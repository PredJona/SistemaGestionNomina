using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmSolicitudesAusencia : Form
    {
        private readonly AbsenceRequestService service = new AbsenceRequestService();
        private readonly EmpleadoService employeeService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private List<SolicitudAusencia> currentRequests = new List<SolicitudAusencia>();

        public FrmSolicitudesAusencia()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmSolicitudesAusencia_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            cmbTipo.Items.Add("Todos");
            cmbTipo.Items.AddRange(AbsenceTypes.All);
            cmbTipo.SelectedIndex = 0;
            cmbEstado.Items.Add("Todos");
            cmbEstado.Items.AddRange(AbsenceStates.All);
            cmbEstado.SelectedIndex = 0;

            bool worker = string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase);
            if (worker)
            {
                cmbDepartamento.Items.Add("Mi información");
                cmbDepartamento.SelectedIndex = 0;
                cmbDepartamento.Enabled = false;
                btnNuevo.Visible = authorizationService.HasPermission(Permissions.OwnAbsencesCreate);
                btnGestionar.Visible = false;
                btnCancelar.Visible = authorizationService.HasPermission(Permissions.OwnAbsencesCancel);
                btnExcel.Visible = false;
                btnPdf.Visible = false;
                lblTitulo.Text = "Mis solicitudes de ausencia";
            }
            else
            {
                List<Departamento> departments = new List<Departamento>();
                departments.Add(new Departamento { IdDepartamento = 0, Nombre = "Todos los departamentos" });
                departments.AddRange(employeeService.GetDepartments());
                cmbDepartamento.DataSource = departments;
                cmbDepartamento.DisplayMember = "Nombre";
                cmbDepartamento.ValueMember = "IdDepartamento";
                btnNuevo.Visible = authorizationService.HasPermission(Permissions.AbsencesCreate);
                btnGestionar.Visible = authorizationService.HasPermission(Permissions.AbsencesApprove) ||
                    authorizationService.HasPermission(Permissions.AbsencesReject);
                btnCancelar.Visible = authorizationService.HasPermission(Permissions.AbsencesCancel);
                btnExcel.Visible = authorizationService.HasPermission(Permissions.AbsencesExport);
                btnPdf.Visible = btnExcel.Visible;
            }
            LoadRequests();
        }

        private void btnBuscar_Click(object sender, EventArgs e) { LoadRequests(); }

        private void LoadRequests()
        {
            try
            {
                int? departmentId = null;
                if (cmbDepartamento.SelectedValue is int && (int)cmbDepartamento.SelectedValue > 0)
                    departmentId = (int)cmbDepartamento.SelectedValue;
                currentRequests = service.Search(new AbsenceQuery
                {
                    DepartmentId = departmentId,
                    Type = cmbTipo.SelectedIndex > 0 ? Convert.ToString(cmbTipo.SelectedItem) : string.Empty,
                    Status = cmbEstado.SelectedIndex > 0 ? Convert.ToString(cmbEstado.SelectedItem) : string.Empty,
                    From = dtpDesde.Checked ? (DateTime?)dtpDesde.Value.Date : null,
                    To = dtpHasta.Checked ? (DateTime?)dtpHasta.Value.Date : null,
                    Search = txtBuscar.Text.Trim()
                });
                dgvSolicitudes.Rows.Clear();
                for (int i = 0; i < currentRequests.Count; i++)
                {
                    SolicitudAusencia item = currentRequests[i];
                    dgvSolicitudes.Rows.Add(item.IdSolicitud, item.CodigoEmpleado, item.EmpleadoNombre,
                        item.DepartamentoNombre, item.Tipo, item.FechaInicio.ToString("dd/MM/yyyy"),
                        item.FechaFin.ToString("dd/MM/yyyy"), item.Estado, item.UsuarioSolicitante,
                        item.FechaCreacion.ToString("dd/MM/yyyy HH:mm"));
                }
                lblResultado.Text = currentRequests.Count + " solicitudes";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (FrmSolicitudAusenciaDetalle form = new FrmSolicitudAusenciaDetalle())
            {
                if (form.ShowDialog() == DialogResult.OK) LoadRequests();
            }
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            int id = SelectedRequestId();
            if (id == 0) return;
            try
            {
                using (FrmSolicitudAusenciaDetalle form = new FrmSolicitudAusenciaDetalle(service.GetById(id)))
                    form.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnGestionar_Click(object sender, EventArgs e) { OpenManagement(false); }
        private void btnCancelar_Click(object sender, EventArgs e) { OpenManagement(true); }

        private void OpenManagement(bool cancelOnly)
        {
            int id = SelectedRequestId();
            if (id == 0) return;
            using (FrmGestionAusencias form = new FrmGestionAusencias(id, cancelOnly))
            {
                if (form.ShowDialog() == DialogResult.OK) LoadRequests();
            }
        }

        private int SelectedRequestId()
        {
            if (dgvSolicitudes.CurrentRow == null || dgvSolicitudes.CurrentRow.Cells["IdSolicitud"].Value == null)
            {
                MessageBox.Show("Seleccione una solicitud.", "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            return Convert.ToInt32(dgvSolicitudes.CurrentRow.Cells["IdSolicitud"].Value);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (currentRequests.Count > 0) UiFactory.ShowExported(excelExportService.ExportarAusencias(currentRequests));
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (currentRequests.Count > 0) UiFactory.ShowExported(pdfExportService.ExportarAusencias(currentRequests));
        }
    }
}
