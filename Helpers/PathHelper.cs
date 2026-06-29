using System;
using System.IO;
using System.Windows.Forms;

namespace SistemaGestionNomina.Helpers
{
    public static class PathHelper
    {
        public static string Timestamp()
        {
            return DateTime.Now.ToString("yyyy_MM_dd_HHmm");
        }

        public static string RequestExportPath(string prefix, string extension, string filter)
        {
            string cleanExtension = NormalizeExtension(extension);
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Guardar exportación";
                dialog.FileName = BuildExportFileName(prefix, cleanExtension);
                dialog.Filter = filter + "|Todos los archivos (*.*)|*.*";
                dialog.DefaultExt = cleanExtension.TrimStart('.');
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                dialog.InitialDirectory = GetSafeInitialDirectory();

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
            }
        }

        public static string BuildUserDocumentPath(string prefix, string extension)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = Path.GetTempPath();
            }

            Directory.CreateDirectory(folder);
            return Path.Combine(folder, BuildExportFileName(prefix, NormalizeExtension(extension)));
        }

        private static string BuildExportFileName(string prefix, string extension)
        {
            return SanitizeFileName(prefix) + "_" + Timestamp() + extension;
        }

        private static string NormalizeExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return ".dat";
            }

            return extension.StartsWith(".") ? extension : "." + extension;
        }

        private static string GetSafeInitialDirectory()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Directory.Exists(documents) ? documents : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        private static string SanitizeFileName(string value)
        {
            string safe = string.IsNullOrWhiteSpace(value) ? "Exportacion" : value.Trim();
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safe = safe.Replace(c, '_');
            }

            return safe.Replace(' ', '_');
        }
    }
}
