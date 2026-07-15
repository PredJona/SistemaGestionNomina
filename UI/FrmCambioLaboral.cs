using System;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.UI
{
    public partial class FrmCambioLaboral : Form
    {
        public EmployeeChangeContext ChangeContext { get; private set; }

        public FrmCambioLaboral() : this(null) { }

        public FrmCambioLaboral(Empleado employee)
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
            if (employee != null)
                lblEmpleado.Text = employee.Codigo + " - " + employee.NombreCompleto;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Indique el motivo del cambio laboral.", "Cambio laboral",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }
            ChangeContext = new EmployeeChangeContext
            {
                EffectiveDate = dtpFechaEfectiva.Value.Date,
                Reason = txtMotivo.Text.Trim()
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
