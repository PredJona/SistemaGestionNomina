using System;
using System.IO;

namespace SistemaGestionNomina.Helpers
{
    public static class PathHelper
    {
        public static string EnsureExportFolder(string relativeFolder)
        {
            string folder = Path.Combine(GetApplicationFolder(), relativeFolder);
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static string Timestamp()
        {
            return DateTime.Now.ToString("yyyy_MM_dd_HHmm");
        }

        private static string GetApplicationFolder()
        {
            string location = typeof(PathHelper).Assembly.Location;
            if (!string.IsNullOrWhiteSpace(location))
            {
                return Path.GetDirectoryName(location);
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
