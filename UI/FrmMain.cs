using System;
using System.ComponentModel;
using System.Windows.Forms;
using FontAwesome.Sharp;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmMain : Form
    {
        private Usuario currentUser;
        private Form activeChild;
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public bool LogoutRequested { get; private set; }

        public FrmMain()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        public FrmMain(Usuario usuario)
            : this()
        {
            if (usuario == null) throw new ArgumentNullException("usuario");
            currentUser = usuario;
            if (!SessionContext.IsAuthenticated) SessionContext.Begin(usuario);
            ConfigureRuntime();
        }

        public void OpenChildForm(Form childForm)
        {
            if (activeChild != null)
            {
                activeChild.Close();
                activeChild.Dispose();
            }

            activeChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(childForm);
            childForm.Show();
        }

        private void ConfigureRuntime()
        {
            lblUsuarioActual.Text = currentUser.NombreUsuario + " | " + currentUser.Rol;
            ApplyPermissions();
            if (!IsDesignTime())
            {
                OpenInitialModule();
            }
        }

        private void ApplyPermissions()
        {
            btnDashboard.Visible = authorizationService.HasPermission(Permissions.DashboardView);
            btnEmpleados.Visible = authorizationService.HasPermission(Permissions.EmployeesView);
            btnAsistencia.Visible = authorizationService.HasPermission(Permissions.AttendanceView);
            btnNomina.Visible = authorizationService.HasPermission(Permissions.PayrollView);
            btnComprobantes.Visible = authorizationService.HasPermission(Permissions.PayslipsView);
            btnReportes.Visible = authorizationService.HasPermission(Permissions.ReportsView);
            btnConfiguracion.Visible = authorizationService.HasPermission(Permissions.ConfigurationView);
            btnAuditoria.Visible = authorizationService.HasPermission(Permissions.AuditView);
        }

        private void OpenInitialModule()
        {
            if (btnDashboard.Visible) OpenDashboard();
            else if (btnEmpleados.Visible) OpenAuthorized(Permissions.EmployeesView, btnEmpleados, new FrmEmpleados());
            else if (btnNomina.Visible) OpenAuthorized(Permissions.PayrollView, btnNomina, new FrmNomina());
            else if (btnAsistencia.Visible) OpenAuthorized(Permissions.AttendanceView, btnAsistencia, new FrmAsistencia());
            else
            {
                SetActiveButton(btnAcercaDe);
                OpenChildForm(new FrmAcercaDe());
            }
        }

        private static bool IsDesignTime()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            OpenDashboard();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.EmployeesView, btnEmpleados, new FrmEmpleados());
        }

        private void btnAsistencia_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.AttendanceView, btnAsistencia, new FrmAsistencia());
        }

        private void btnNomina_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.PayrollView, btnNomina, new FrmNomina());
        }

        private void btnComprobantes_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.PayslipsView, btnComprobantes, new FrmComprobantes());
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.ReportsView, btnReportes, new FrmReportes());
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.ConfigurationView, btnConfiguracion, new FrmConfiguracion());
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAcercaDe);
            OpenChildForm(new FrmAcercaDe());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            auditTrailService.RegistrarAccion("Seguridad", "Cerrar sesión", string.Empty);
            SessionContext.Clear();
            LogoutRequested = true;
            Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea salir de la aplicación?",
                "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                auditTrailService.RegistrarAccion("Seguridad", "Salir de la aplicación", string.Empty);
                SessionContext.Clear();
                Application.Exit();
            }
        }

        private void btnAuditoria_Click(object sender, EventArgs e)
        {
            OpenAuthorized(Permissions.AuditView, btnAuditoria, new FrmAuditoria());
        }

        private void OpenDashboard()
        {
            authorizationService.DemandPermission(Permissions.DashboardView);
            SetActiveButton(btnDashboard);
            OpenChildForm(new FrmDashboard(
                delegate { OpenChildForm(new FrmEmpleados()); },
                delegate { OpenChildForm(new FrmNomina()); }));
        }

        private void OpenAuthorized(string permission, IconButton button, Form form)
        {
            try
            {
                authorizationService.DemandPermission(permission);
                SetActiveButton(button);
                OpenChildForm(form);
            }
            catch (UnauthorizedAccessException ex)
            {
                form.Dispose();
                MessageBox.Show(ex.Message, "Acceso restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetActiveButton(IconButton button)
        {
            ControlStyleHelper.SetActiveSidebarButton(panelSidebar, button);
        }
    }
}
