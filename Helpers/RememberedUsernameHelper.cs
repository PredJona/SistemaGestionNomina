using System;
using System.IO;

namespace SistemaGestionNomina.Helpers
{
    public static class RememberedUsernameHelper
    {
        private static string FilePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ProyFinal_LPI_Eq01_NomiCore", "usuario.recordado");
            }
        }

        public static string Load()
        {
            try
            {
                return File.Exists(FilePath) ? File.ReadAllText(FilePath).Trim() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void Save(string username)
        {
            string folder = Path.GetDirectoryName(FilePath);
            Directory.CreateDirectory(folder);
            File.WriteAllText(FilePath, (username ?? string.Empty).Trim());
        }

        public static void Clear()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
    }
}
