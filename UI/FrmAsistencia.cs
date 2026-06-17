using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmAsistencia : Form
    {
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly AsistenciaService asistenciaService = new AsistenciaService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private List<Asistencia> currentItems = new List<Asistencia>();

        public FrmAsistencia()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmAsistencia_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            LoadEmployees();
            LoadAttendance();
        }

        private void LoadEmployees()
        {
            List<Empleado> employees = empleadoService.GetActive(null);
            cmbEmpleado.DataSource = new List<Empleado>(employees);
            cmbEmpleado.DisplayMember = "NombreCompleto";
            cmbEmpleado.ValueMember = "IdEmpleado";

            List<Empleado> filters = new List<Empleado>();
            filters.Add(new Empleado { IdEmpleado = 0, Nombre = "Todos", Apellido = "los empleados" });
            filters.AddRange(employees);
            cmbFiltroEmpleado.DataSource = filters;
            cmbFiltroEmpleado.DisplayMember = "NombreCompleto";
            cmbFiltroEmpleado.ValueMember = "IdEmpleado";
        }

        private void LoadAttendance()
        {
            int? employeeId = null;
            if (cmbFiltroEmpleado.SelectedValue is int && (int)cmbFiltroEmpleado.SelectedValue > 0)
            {
                employeeId = (int)cmbFiltroEmpleado.SelectedValue;
            }

            string status = cmbFiltroEstado.SelectedItem == null || cmbFiltroEstado.SelectedItem.ToString() == "Todos"
                ? string.Empty
                : cmbFiltroEstado.SelectedItem.ToString();

            currentItems = asistenciaService.GetAll(dtpFiltroInicio.Value.Date, dtpFiltroFin.Value.Date, employeeId, status);
            dgvAsistencia.Rows.Clear();
            foreach (Asistencia a in currentItems)
            {
                dgvAsistencia.Rows.Add(a.EmpleadoNombre, a.Fecha.ToString("dd/MM/yyyy"),
                    a.HoraEntrada.HasValue ? a.HoraEntrada.Value.ToString(@"hh\:mm") : "--",
                    a.HoraSalida.HasValue ? a.HoraSalida.Value.ToString(@"hh\:mm") : "--",
                    a.HorasTrabajadas.ToString("0.00"), a.Estado);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un empleado.", "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Asistencia asistencia = new Asistencia();
                asistencia.IdEmpleado = Convert.ToInt32(cmbEmpleado.SelectedValue);
                asistencia.Fecha = dtpFecha.Value.Date;
                asistencia.HoraEntrada = dtpHoraEntrada.Value.TimeOfDay;
                asistencia.HoraSalida = dtpHoraSalida.Value.TimeOfDay;
                asistencia.Estado = cmbEstado.SelectedItem.ToString();
                asistenciaService.Register(asistencia);
                MessageBox.Show("Asistencia registrada correctamente.", "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAttendance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            UiFactory.ShowExported(excelExportService.ExportarAsistencias(currentItems));
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            UiFactory.ShowExported(pdfExportService.ExportarAsistencias(currentItems));
        }

        private void btnCargarReloj_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad pendiente para completar por otro integrante.",
                "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
