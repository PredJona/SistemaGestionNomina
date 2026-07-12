using System.Windows.Forms;
using SistemaGestionNomina.Helpers;

namespace SistemaGestionNomina.UI
{
    public partial class FrmCambiarPassword : Form
    {
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
    }
}
