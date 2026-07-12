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
    public partial class FrmNomina : Form
    {
        private readonly NominaService nominaService = new NominaService();
        private readonly EmpleadoService empleadoService = new EmpleadoService();
        private readonly ExcelExportService excelExportService = new ExcelExportService();
        private readonly PdfExportService pdfExportService = new PdfExportService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private Nomina currentNomina;

        public FrmNomina()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmNomina_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            btnCalcular.Visible = authorizationService.HasPermission(Permissions.PayrollCalculate);
            btnConfirmarPago.Visible = authorizationService.HasPermission(Permissions.PayrollConfirm);
            btnExportarExcel.Visible = authorizationService.HasPermission(Permissions.PayrollExport);
            btnExportarPdf.Visible = authorizationService.HasPermission(Permissions.PayrollExport);
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            List<Departamento> departments = new List<Departamento>();
            departments.Add(new Departamento { IdDepartamento = 0, Nombre = "Todos los departamentos" });
            departments.AddRange(empleadoService.GetDepartments());
            cmbDepartamento.DataSource = departments;
            cmbDepartamento.DisplayMember = "Nombre";
            cmbDepartamento.ValueMember = "IdDepartamento";
            cmbPeriodo.Text = "Nómina " + DateTime.Today.ToString("MMMM yyyy");
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.ValidateDateRange(dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date))
            {
                return;
            }

            try
            {
                int? dep = null;
                if (cmbDepartamento.SelectedValue is int && (int)cmbDepartamento.SelectedValue > 0)
                {
                    dep = (int)cmbDepartamento.SelectedValue;
                }
                currentNomina = nominaService.CalcularNomina(dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, dep);
                if (!string.IsNullOrWhiteSpace(cmbPeriodo.Text))
                {
                    currentNomina.PeriodoNombre = cmbPeriodo.Text.Trim();
                }
                LoadNominaGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadNominaGrid()
        {
            dgvNominaDetalle.Rows.Clear();
            foreach (NominaDetalle d in currentNomina.Detalles)
            {
                dgvNominaDetalle.Rows.Add(d.CodigoEmpleado, d.EmpleadoNombre, "B/. " + d.SueldoBase.ToString("0.00"),
                    "B/. " + d.Bonos.ToString("0.00"), d.HorasExtra.ToString("0.00"),
                    "B/. " + d.TotalDeducciones.ToString("0.00"), "B/. " + d.NetoPagar.ToString("0.00"));
            }

            lblTotalIngresos.Text = "Ingresos: B/. " + currentNomina.TotalIngresos.ToString("0.00");
            lblTotalDeducciones.Text = "Deducciones: B/. " + currentNomina.TotalDeducciones.ToString("0.00");
            lblTotalNeto.Text = "B/. " + currentNomina.TotalNeto.ToString("0.00");
            lblProcesados.Text = currentNomina.Detalles.Count + " empleados procesados";
        }

        private void btnConfirmarPago_Click(object sender, EventArgs e)
        {
            if (!EnsureCalculated()) return;
            if (MessageBox.Show("¿Desea confirmar el pago y generar comprobantes?", "Confirmar pago",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int id = nominaService.ConfirmarPago(currentNomina, dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date);
                MessageBox.Show("Nómina confirmada correctamente. Id: " + id + "\nSe generaron los comprobantes.",
                    "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (EnsureCalculated()) UiFactory.ShowExported(excelExportService.ExportarNomina(currentNomina));
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (EnsureCalculated()) UiFactory.ShowExported(pdfExportService.ExportarNomina(currentNomina));
        }

        private bool EnsureCalculated()
        {
            if (currentNomina != null && currentNomina.Detalles.Count > 0) return true;
            MessageBox.Show("Primero calcule la nómina.", "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }
}
