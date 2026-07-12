using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmMisAsistencias : Form
    {
        private readonly EmployeePortalService portalService = new EmployeePortalService();

        public FrmMisAsistencias()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmMisAsistencias_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpHasta.Value = DateTime.Today;
            cmbEstado.SelectedIndex = 0;
            LoadAttendance();
        }

        private void btnFiltrar_Click(object sender, EventArgs e) { LoadAttendance(); }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;
            cmbEstado.SelectedIndex = 0;
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            try
            {
                DateTime? start = dtpDesde.Checked ? dtpDesde.Value.Date : (DateTime?)null;
                DateTime? end = dtpHasta.Checked ? dtpHasta.Value.Date : (DateTime?)null;
                List<Asistencia> items = portalService.GetMyAttendance(start, end,
                    Convert.ToString(cmbEstado.SelectedItem));

                dgvAsistencias.Rows.Clear();
                foreach (Asistencia item in items)
                {
                    dgvAsistencias.Rows.Add(item.Fecha.ToString("dd/MM/yyyy"),
                        item.HoraEntrada.HasValue ? item.HoraEntrada.Value.ToString(@"hh\:mm") : "-",
                        item.HoraSalida.HasValue ? item.HoraSalida.Value.ToString(@"hh\:mm") : "-",
                        item.HorasTrabajadas.ToString("0.00"), item.Estado);
                }

                EmployeeAttendanceSummaryViewModel summary = EmployeePortalService.SummarizeAttendance(items);
                lblPuntualesValor.Text = summary.Puntuales + "\nPuntuales";
                lblTardanzasValor.Text = summary.Tardanzas + "\nTardanzas";
                lblFaltasValor.Text = summary.Faltas + "\nFaltas";
                lblPermisosValor.Text = summary.Permisos + "\nPermisos";
                lblHorasExtraValor.Text = summary.HorasExtra.ToString("0.00") + " h\nHoras extra";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mis asistencias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
