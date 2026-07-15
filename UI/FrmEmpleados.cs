using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.UI
{
    public partial class FrmEmpleados : Form
    {
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private int selectedId;
        private List<Empleado> currentEmployees = new List<Empleado>();

        public FrmEmpleados()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            btnNuevo.Visible = authorizationService.HasPermission(Permissions.EmployeesCreate);
            btnEditar.Visible = authorizationService.HasPermission(Permissions.EmployeesEdit);
            btnDesactivar.Visible = authorizationService.HasPermission(Permissions.EmployeesDeactivate);
            btnExportarExcel.Visible = authorizationService.HasPermission(Permissions.EmployeesExport);
            btnExportarPdf.Visible = authorizationService.HasPermission(Permissions.EmployeesExport);
            btnHistorialLaboral.Visible = authorizationService.HasPermission(Permissions.EmployeeHistoryView);
            LoadDepartments();
            LoadEmployees();
        }

        private void LoadDepartments()
        {
            List<Departamento> departments = empleadoService.GetDepartments();
            cmbDepartamento.DataSource = new List<Departamento>(departments);
            cmbDepartamento.DisplayMember = "Nombre";
            cmbDepartamento.ValueMember = "IdDepartamento";

            List<Departamento> filters = new List<Departamento>();
            filters.Add(new Departamento { IdDepartamento = 0, Nombre = "Todos los departamentos" });
            filters.AddRange(departments);
            cmbFiltroDepartamento.DataSource = filters;
            cmbFiltroDepartamento.DisplayMember = "Nombre";
            cmbFiltroDepartamento.ValueMember = "IdDepartamento";
        }

        private void LoadEmployees()
        {
            int? department = null;
            if (cmbFiltroDepartamento.SelectedValue is int && (int)cmbFiltroDepartamento.SelectedValue > 0)
            {
                department = (int)cmbFiltroDepartamento.SelectedValue;
            }

            string status = cmbFiltroEstado.SelectedItem == null || cmbFiltroEstado.SelectedItem.ToString() == "Todos"
                ? string.Empty
                : cmbFiltroEstado.SelectedItem.ToString();

            currentEmployees = empleadoService.GetAll(txtBuscar.Text, department, status);
            dgvEmpleados.Rows.Clear();
            foreach (Empleado e in currentEmployees)
            {
                dgvEmpleados.Rows.Add(e.IdEmpleado, e.Codigo, e.NombreCompleto, e.Cedula, e.Cargo,
                    e.DepartamentoNombre, "B/. " + e.SalarioBase.ToString("0.00"), e.Estado, "Editar");
            }
        }

        private void dgvEmpleados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null || dgvEmpleados.CurrentRow.Cells["IdEmpleado"].Value == null)
            {
                return;
            }

            selectedId = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["IdEmpleado"].Value);
            Empleado empleado = empleadoService.GetById(selectedId);
            if (empleado == null)
            {
                return;
            }

            txtCodigo.Text = empleado.Codigo;
            txtNombre.Text = empleado.Nombre;
            txtApellido.Text = empleado.Apellido;
            txtCedula.Text = empleado.Cedula;
            txtCargo.Text = empleado.Cargo;
            cmbDepartamento.SelectedValue = empleado.IdDepartamento;
            txtSalario.Text = empleado.SalarioBase.ToString("0.00");
            cmbEstado.SelectedItem = empleado.Estado;
            dtpIngreso.Value = empleado.FechaIngreso;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            decimal salary;
            if (!ValidationHelper.RequireText(txtCodigo, "Código") ||
                !ValidationHelper.RequireText(txtNombre, "Nombre") ||
                !ValidationHelper.RequireText(txtApellido, "Apellido") ||
                !ValidationHelper.RequireText(txtCedula, "Cédula") ||
                !ValidationHelper.RequireText(txtCargo, "Cargo") ||
                !ValidationHelper.TryPositiveDecimal(txtSalario, "Salario base", out salary))
            {
                return;
            }

            if (cmbDepartamento.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un departamento.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbEstado.SelectedItem == null)
            {
                MessageBox.Show("Seleccione el estado del empleado.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Empleado empleado = new Empleado();
                empleado.IdEmpleado = selectedId;
                empleado.Codigo = txtCodigo.Text.Trim();
                empleado.Nombre = txtNombre.Text.Trim();
                empleado.Apellido = txtApellido.Text.Trim();
                empleado.Cedula = txtCedula.Text.Trim();
                empleado.Cargo = txtCargo.Text.Trim();
                empleado.IdDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue);
                empleado.SalarioBase = salary;
                empleado.Estado = cmbEstado.SelectedItem.ToString();
                empleado.FechaIngreso = dtpIngreso.Value.Date;
                EmployeeChangeContext changeContext = null;
                if (selectedId > 0)
                {
                    Empleado original = empleadoService.GetById(selectedId);
                    if (HasTrackedChanges(original, empleado))
                    {
                        using (FrmCambioLaboral dialog = new FrmCambioLaboral(original))
                        {
                            if (dialog.ShowDialog() != DialogResult.OK) return;
                            changeContext = dialog.ChangeContext;
                        }
                    }
                }
                EmployeeSaveResult result = empleadoService.Save(empleado, changeContext);
                string message = result.HasScheduledChanges
                    ? "El cambio laboral quedó programado para la fecha indicada."
                    : "Empleado guardado correctamente.";
                MessageBox.Show(message, "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Seleccione un empleado.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("¿Desea desactivar el empleado seleccionado?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Empleado employee = empleadoService.GetById(selectedId);
                    using (FrmCambioLaboral dialog = new FrmCambioLaboral(employee))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK) return;
                        EmployeeSaveResult result = empleadoService.Deactivate(selectedId, dialog.ChangeContext);
                        MessageBox.Show(result.HasScheduledChanges
                            ? "La desactivación quedó programada." : "Empleado desactivado correctamente.",
                            "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ClearForm();
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (currentEmployees.Count == 0)
            {
                MessageBox.Show("No hay empleados para exportar.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UiFactory.ShowExported(excelExportService.ExportarEmpleados(currentEmployees));
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (currentEmployees.Count == 0)
            {
                MessageBox.Show("No hay empleados para exportar.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UiFactory.ShowExported(pdfExportService.ExportarEmpleados(currentEmployees));
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && cmbFiltroDepartamento.DataSource != null)
            {
                LoadEmployees();
            }
        }

        private void btnHistorialLaboral_Click(object sender, EventArgs e)
        {
            using (FrmHistorialEmpleado form = new FrmHistorialEmpleado(selectedId > 0 ? (int?)selectedId : null))
                form.ShowDialog();
        }

        private static bool HasTrackedChanges(Empleado original, Empleado updated)
        {
            return original != null && (original.SalarioBase != updated.SalarioBase ||
                original.IdDepartamento != updated.IdDepartamento ||
                !string.Equals(original.Cargo, updated.Cargo, StringComparison.Ordinal) ||
                !string.Equals(original.Estado, updated.Estado, StringComparison.OrdinalIgnoreCase));
        }

        private void ClearForm()
        {
            selectedId = 0;
            txtCodigo.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtCedula.Clear();
            txtCargo.Clear();
            txtSalario.Clear();
            if (cmbDepartamento.Items.Count > 0)
            {
                cmbDepartamento.SelectedIndex = 0;
            }
            cmbEstado.SelectedIndex = 0;
            dtpIngreso.Value = DateTime.Today;
        }
    }
}
