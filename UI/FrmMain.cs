using System;
using System.ComponentModel;
using System.Windows.Forms;
using FontAwesome.Sharp;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.UI
{
    public partial class FrmMain : Form
    {
        private Usuario currentUser;
        private Form activeChild;

        public bool LogoutRequested { get; private set; }

        public FrmMain()
            : this(new Usuario { NombreUsuario = "admin", Rol = "Admin" })
        {
        }

        public FrmMain(Usuario usuario)
        {
            currentUser = usuario;
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
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
            if (!IsDesignTime())
            {
                OpenDashboard();
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
            SetActiveButton(btnEmpleados);
            OpenChildForm(new FrmEmpleados());
        }

        private void btnAsistencia_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAsistencia);
            OpenChildForm(new FrmAsistencia());
        }

        private void btnNomina_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNomina);
            OpenChildForm(new FrmNomina());
        }

        private void btnComprobantes_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnComprobantes);
            OpenChildForm(new FrmComprobantes());
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReportes);
            OpenChildForm(new FrmReportes());
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnConfiguracion);
            OpenChildForm(new FrmConfiguracion());
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAcercaDe);
            OpenChildForm(new FrmAcercaDe());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            LogoutRequested = true;
            Close();
        }

        private void OpenDashboard()
        {
            SetActiveButton(btnDashboard);
            OpenChildForm(new FrmDashboard(
                delegate { OpenChildForm(new FrmEmpleados()); },
                delegate { OpenChildForm(new FrmNomina()); }));
        }

        private void SetActiveButton(IconButton button)
        {
            ControlStyleHelper.SetActiveSidebarButton(panelSidebar, button);
        }
    }
}
