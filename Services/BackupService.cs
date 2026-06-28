using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SistemaGestionNomina.Data;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio reservado para crear copias de seguridad de la base de datos.
    /// </summary>
    public class BackupService
    {
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        /// <summary>
        /// Crea una copia de seguridad de nomina.db en la ruta indicada.
        /// </summary>
        public string CrearBackup(string carpetaDestino)
        {
            string source = SQLiteConnectionFactory.DatabasePath;
            if (!File.Exists(source))
            {
                throw new FileNotFoundException("No se encontró la base de datos para respaldar.", source);
            }

            string destinationFolder = string.IsNullOrWhiteSpace(carpetaDestino)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups")
                : carpetaDestino;

            Directory.CreateDirectory(destinationFolder);
            string destination = Path.Combine(destinationFolder, "nomina_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".db");
            File.Copy(source, destination, false);

            string hash = ComputeSha256(destination);
            File.WriteAllText(destination + ".sha256", hash, Encoding.ASCII);
            auditTrailService.RegistrarCambio("admin", "Backup", "Crear", Path.GetFileName(destination));
            return destination;
        }

        /// <summary>
        /// Lista respaldos disponibles ordenados del más reciente al más antiguo.
        /// </summary>
        public List<string> ListarBackups(string carpeta)
        {
            string folder = string.IsNullOrWhiteSpace(carpeta)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups")
                : carpeta;

            List<string> backups = new List<string>();
            if (!Directory.Exists(folder))
            {
                return backups;
            }

            string[] files = Directory.GetFiles(folder, "nomina_*.db");
            Array.Sort(files);
            Array.Reverse(files);
            backups.AddRange(files);
            return backups;
        }

        /// <summary>
        /// Verifica que el hash del backup coincida con su archivo .sha256.
        /// </summary>
        public bool VerificarBackup(string rutaBackup)
        {
            if (string.IsNullOrWhiteSpace(rutaBackup) || !File.Exists(rutaBackup))
            {
                return false;
            }

            string hashFile = rutaBackup + ".sha256";
            if (!File.Exists(hashFile))
            {
                return false;
            }

            string expected = File.ReadAllText(hashFile).Trim();
            return string.Equals(expected, ComputeSha256(rutaBackup), StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeSha256(string path)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream stream = File.OpenRead(path))
            {
                byte[] hash = sha.ComputeHash(stream);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
