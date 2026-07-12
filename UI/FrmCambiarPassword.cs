using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmCambiarPassword : Form
    {
        private readonly AccountService accountService = new AccountService();

        public FrmCambiarPassword()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void ClearFields()
        {
            txtActual.Clear();
            txtNueva.Clear();
            txtConfirmar.Clear();
            txtActual.Focus();
        }

        private void btnGuardar_Click(object sender, System.EventArgs e)
        {
            try
            {
                accountService.ChangeOwnPassword(txtActual.Text, txtNueva.Text, txtConfirmar.Text);
                MessageBox.Show("La contrasena se actualizo correctamente.", "Cuenta",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Cambiar contrasena", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
