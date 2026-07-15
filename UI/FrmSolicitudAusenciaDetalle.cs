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
    public partial class FrmSolicitudAusenciaDetalle : Form
    {
        private readonly SolicitudAusencia existingRequest;
        private readonly AbsenceRequestService absenceService = new AbsenceRequestService();
        private readonly EmpleadoService employeeService = new EmpleadoService();
        public bool RequestCreated { get; private set; }

        public FrmSolicitudAusenciaDetalle() : this(null) { }

        public FrmSolicitudAusenciaDetalle(SolicitudAusencia item)
        {
            existingRequest = item;
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmSolicitudAusenciaDetalle_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            cmbTipo.DataSource = new List<string>(AbsenceTypes.All);
            if (existingRequest != null)
            {
                Text = "Detalle de solicitud";
                lblTitulo.Text = "Detalle de la solicitud";
                cmbEmpleado.Items.Add(existingRequest.CodigoEmpleado + " - " + existingRequest.EmpleadoNombre);
                cmbEmpleado.SelectedIndex = 0;
                cmbTipo.SelectedItem = existingRequest.Tipo;
                dtpInicio.Value = existingRequest.FechaInicio;
                dtpFin.Value = existingRequest.FechaFin;
                txtMotivo.Text = existingRequest.Motivo;
                txtResolucion.Text = existingRequest.ObservacionResolucion;
                lblEstado.Text = "Estado: " + existingRequest.Estado;
                SetReadOnly();
                return;
            }

            lblEstado.Text = "Estado inicial: Pendiente";
            if (string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase))
            {
                cmbEmpleado.Items.Add("Empleado asociado a la sesión");
                cmbEmpleado.SelectedIndex = 0;
                cmbEmpleado.Enabled = false;
            }
            else
            {
                List<Empleado> employees = employeeService.GetActive(null);
                cmbEmpleado.DataSource = employees;
                cmbEmpleado.DisplayMember = "NombreCompleto";
                cmbEmpleado.ValueMember = "IdEmpleado";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string type = Convert.ToString(cmbTipo.SelectedItem);
                if (string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase))
                {
                    absenceService.CreateOwn(new OwnAbsenceRequest
                    {
                        Type = type,
                        StartDate = dtpInicio.Value.Date,
                        EndDate = dtpFin.Value.Date,
                        Reason = txtMotivo.Text
                    });
                }
                else
                {
                    if (!(cmbEmpleado.SelectedValue is int))
                        throw new ArgumentException("Seleccione un empleado.");
                    absenceService.CreateForEmployee(new SolicitudAusencia
                    {
                        IdEmpleado = (int)cmbEmpleado.SelectedValue,
                        Tipo = type,
                        FechaInicio = dtpInicio.Value.Date,
                        FechaFin = dtpFin.Value.Date,
                        Motivo = txtMotivo.Text
                    });
                }
                RequestCreated = true;
                MessageBox.Show("Solicitud registrada en estado Pendiente.", "Ausencias",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ausencias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) { Close(); }

        private void SetReadOnly()
        {
            cmbEmpleado.Enabled = false;
            cmbTipo.Enabled = false;
            dtpInicio.Enabled = false;
            dtpFin.Enabled = false;
            txtMotivo.ReadOnly = true;
            txtResolucion.ReadOnly = true;
            btnGuardar.Visible = false;
        }
    }
}
