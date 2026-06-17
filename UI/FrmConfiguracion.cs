using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmConfiguracion : Form
    {
        private readonly ConfiguracionService configuracionService = new ConfiguracionService();

        public FrmConfiguracion()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmConfiguracion_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadSettings();
            }
        }

        private void LoadSettings()
        {
            nudSeguroSocial.Value = Clamp(configuracionService.GetValue("SeguroSocial", 9.75m), nudSeguroSocial);
            nudISR.Value = Clamp(configuracionService.GetValue("ISR", 10m), nudISR);
            nudSeguroEducativo.Value = Clamp(configuracionService.GetValue("SeguroEducativo", 1.25m), nudSeguroEducativo);
            nudRecargoHoraExtra.Value = Clamp(configuracionService.GetValue("RecargoHoraExtra", 1.25m), nudRecargoHoraExtra);
            nudHorasMensualesBase.Value = Clamp(configuracionService.GetValue("HorasMensualesBase", 160m), nudHorasMensualesBase);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Dictionary<string, decimal> values = new Dictionary<string, decimal>();
            values["SeguroSocial"] = nudSeguroSocial.Value;
            values["ISR"] = nudISR.Value;
            values["SeguroEducativo"] = nudSeguroEducativo.Value;
            values["RecargoHoraExtra"] = nudRecargoHoraExtra.Value;
            values["HorasMensualesBase"] = nudHorasMensualesBase.Value;
            configuracionService.SaveDefaults(values);
            MessageBox.Show("Configuración guardada correctamente.", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDescartar_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private static decimal Clamp(decimal value, NumericUpDown control)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }
    }
}
