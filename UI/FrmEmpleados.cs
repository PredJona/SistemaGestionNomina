using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmEmpleados : Form
    {
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
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
                empleadoService.Save(empleado);
                MessageBox.Show("Empleado guardado correctamente.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                empleadoService.Deactivate(selectedId);
                ClearForm();
                LoadEmployees();
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            UiFactory.ShowExported(excelExportService.ExportarEmpleados(currentEmployees));
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            UiFactory.ShowExported(pdfExportService.ExportarEmpleados(currentEmployees));
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && cmbFiltroDepartamento.DataSource != null)
            {
                LoadEmployees();
            }
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
