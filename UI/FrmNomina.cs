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
        private readonly PayrollLifecycleService payrollLifecycleService = new PayrollLifecycleService();
        private Nomina currentNomina;
        private int? currentNominaId;

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
            btnPagar.Visible = authorizationService.HasPermission(Permissions.PayrollPay);
            btnAnular.Visible = authorizationService.HasPermission(Permissions.PayrollAnnul);
            btnRecalcular.Visible = authorizationService.HasPermission(Permissions.PayrollRecalculate);
            btnVerHistorial.Visible = authorizationService.HasPermission(Permissions.PayrollHistoryView);
            LoadDepartments();
            ActualizarEstadoBotones();
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
            lblEstadoNomina.Text = "Estado: " + (currentNomina.Estado ?? "Calculada");
            ActualizarEstadoBotones();
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
                currentNominaId = id;
                currentNomina.Estado = PayrollStates.Confirmed;
                MessageBox.Show("Nómina confirmada correctamente. Id: " + id + "\nSe generaron los comprobantes.\nEl período ha sido cerrado.",
                    "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            if (!currentNominaId.HasValue)
            {
                MessageBox.Show("Primero debe confirmar la nómina.", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("¿Registrar el pago de esta nómina? Esta acción no se puede deshacer.",
                "Registrar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                nominaService.PagarNomina(currentNominaId.Value);
                currentNomina.Estado = PayrollStates.Paid;
                MessageBox.Show("Pago registrado correctamente.", "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (!currentNominaId.HasValue)
            {
                MessageBox.Show("Primero debe confirmar la nómina.", "Anular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FrmAnularNomina dialogo = new FrmAnularNomina(currentNomina))
            {
                if (dialogo.ShowDialog() != DialogResult.OK) return;

                try
                {
                    nominaService.AnularNomina(currentNominaId.Value, dialogo.Motivo);
                    currentNomina.Estado = PayrollStates.Annulled;
                    MessageBox.Show("Nómina anulada correctamente.\nEl período ha sido reabierto para corrección.",
                        "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActualizarEstadoBotones();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            if (!currentNominaId.HasValue)
            {
                MessageBox.Show("Primero debe anular la nómina.", "Recalcular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int? dep = null;
                if (cmbDepartamento.SelectedValue is int && (int)cmbDepartamento.SelectedValue > 0)
                    dep = (int)cmbDepartamento.SelectedValue;

                Nomina nuevaNomina = nominaService.CalcularNomina(dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, dep);
                if (!string.IsNullOrWhiteSpace(cmbPeriodo.Text))
                    nuevaNomina.PeriodoNombre = cmbPeriodo.Text.Trim();

                int idNueva = nominaService.RecalcularNomina(currentNominaId.Value,
                    dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, nuevaNomina);
                currentNomina = nuevaNomina;
                currentNomina.Estado = PayrollStates.Confirmed;
                currentNominaId = idNueva;
                LoadNominaGrid();
                MessageBox.Show("Nómina recalculada y confirmada correctamente. Id: " + idNueva,
                    "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nómina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVerHistorial_Click(object sender, EventArgs e)
        {
            if (!currentNominaId.HasValue)
            {
                MessageBox.Show("Primero debe confirmar la nómina.", "Historial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<NominaVersion> versiones = nominaService.ObtenerHistorialNomina(currentNominaId.Value);
            if (versiones.Count == 0)
            {
                MessageBox.Show("No hay versiones registradas para esta nómina.", "Historial",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FrmHistorialNomina dialogo = new FrmHistorialNomina(versiones))
            {
                dialogo.ShowDialog();
            }
        }

        private void ActualizarEstadoBotones()
        {
            string estado = currentNomina?.Estado ?? string.Empty;
            bool annulled = string.Equals(estado, PayrollStates.Annulled, StringComparison.OrdinalIgnoreCase);
            btnCalcular.Enabled = authorizationService.HasPermission(Permissions.PayrollCalculate) &&
                (!currentNominaId.HasValue || annulled);
            btnConfirmarPago.Enabled = authorizationService.HasPermission(Permissions.PayrollConfirm) &&
                PayrollStateMachine.CanTransition(estado, PayrollStates.Confirmed);
            btnPagar.Enabled = authorizationService.HasPermission(Permissions.PayrollPay) &&
                PayrollStateMachine.CanTransition(estado, PayrollStates.Paid);
            btnAnular.Enabled = authorizationService.HasPermission(Permissions.PayrollAnnul) &&
                PayrollStateMachine.CanTransition(estado, PayrollStates.Annulled);
            btnRecalcular.Enabled = authorizationService.HasPermission(Permissions.PayrollRecalculate) && annulled;
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
