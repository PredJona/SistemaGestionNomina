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
    public partial class FrmHistorialEmpleado : Form
    {
        private readonly int? initialEmployeeId;
        private readonly EmployeeHistoryService historyService = new EmployeeHistoryService();
        private readonly EmpleadoService employeeService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private List<HistorialEmpleado> currentHistory = new List<HistorialEmpleado>();

        public FrmHistorialEmpleado() : this(null) { }

        public FrmHistorialEmpleado(int? employeeId)
        {
            initialEmployeeId = employeeId;
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmHistorialEmpleado_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            List<Empleado> employees = new List<Empleado>();
            employees.Add(new Empleado
            {
                IdEmpleado = 0, Codigo = "TODOS", Nombre = "Todos los empleados", Apellido = string.Empty,
                Cedula = "N/A", Cargo = "N/A", IdDepartamento = 1, SalarioBase = 1,
                Estado = "Activo", FechaIngreso = DateTime.Today
            });
            employees.AddRange(employeeService.GetAll(string.Empty, null, string.Empty));
            cmbEmpleado.DataSource = employees;
            cmbEmpleado.DisplayMember = "NombreCompleto";
            cmbEmpleado.ValueMember = "IdEmpleado";
            if (initialEmployeeId.HasValue) cmbEmpleado.SelectedValue = initialEmployeeId.Value;
            cmbCampo.Items.Add("Todos");
            cmbCampo.Items.Add(EmployeeHistoryFields.BaseSalary);
            cmbCampo.Items.Add(EmployeeHistoryFields.Position);
            cmbCampo.Items.Add(EmployeeHistoryFields.Department);
            cmbCampo.Items.Add(EmployeeHistoryFields.Status);
            cmbCampo.SelectedIndex = 0;
            btnExcel.Visible = authorizationService.HasPermission(Permissions.EmployeeHistoryExport);
            btnPdf.Visible = btnExcel.Visible;
            LoadHistory();
        }

        private void btnBuscar_Click(object sender, EventArgs e) { LoadHistory(); }

        private void LoadHistory()
        {
            try
            {
                int employeeId = cmbEmpleado.SelectedValue is int ? (int)cmbEmpleado.SelectedValue : 0;
                currentHistory = historyService.Search(new EmployeeHistoryQuery
                {
                    EmployeeId = employeeId > 0 ? (int?)employeeId : null,
                    Field = cmbCampo.SelectedIndex > 0 ? Convert.ToString(cmbCampo.SelectedItem) : string.Empty,
                    User = txtUsuario.Text.Trim(),
                    From = dtpDesde.Checked ? (DateTime?)dtpDesde.Value.Date : null,
                    To = dtpHasta.Checked ? (DateTime?)dtpHasta.Value.Date : null
                });
                dgvHistorial.Rows.Clear();
                for (int i = 0; i < currentHistory.Count; i++)
                {
                    HistorialEmpleado item = currentHistory[i];
                    dgvHistorial.Rows.Add(item.CodigoEmpleado, item.EmpleadoNombre, item.CampoModificado,
                        item.ValorAnterior, item.ValorNuevo, item.FechaEfectiva.ToString("dd/MM/yyyy"),
                        item.FechaCambio.ToString("dd/MM/yyyy HH:mm"), item.UsuarioResponsable,
                        item.Motivo, item.Aplicado ? "Aplicado" : "Programado");
                }
                lblResultado.Text = currentHistory.Count + " cambios encontrados";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Historial laboral", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (currentHistory.Count == 0) return;
            UiFactory.ShowExported(excelExportService.ExportarHistorialEmpleados(currentHistory));
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (currentHistory.Count == 0) return;
            UiFactory.ShowExported(pdfExportService.ExportarHistorialEmpleados(currentHistory));
        }

        private void btnCerrar_Click(object sender, EventArgs e) { Close(); }
    }
}
