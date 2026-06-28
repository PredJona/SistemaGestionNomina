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
        private readonly BackupService backupService = new BackupService();

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

        private void btnBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Seleccione la carpeta donde se guardará el backup.";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    string path = backupService.CrearBackup(dialog.SelectedPath);
                    bool verified = backupService.VerificarBackup(path);
                    MessageBox.Show("Backup generado correctamente:\n" + path + "\n\nVerificación SHA-256: " +
                        (verified ? "correcta" : "no disponible"), "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private static decimal Clamp(decimal value, NumericUpDown control)
        {
            if (value < control.Minimum) return control.Minimum;
            if (value > control.Maximum) return control.Maximum;
            return value;
        }
    }
}
