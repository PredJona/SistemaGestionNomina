using System;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;

namespace SistemaGestionNomina.UI
{
    public partial class FrmAcercaDe : Form
    {
        public FrmAcercaDe()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
