using System;
using System.IO;
using System.Text;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio responsable de enviar comprobantes por correo electrónico.
    /// </summary>
    public class EmailService
    {
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        /// <summary>
        /// Envía un comprobante generado a un destinatario.
        /// </summary>
        public void EnviarComprobante(string destinatario, string rutaPdf)
        {
            CrearBorradorComprobante(destinatario, rutaPdf, "Comprobante de pago");
        }

        /// <summary>
        /// Crea un borrador .eml con el comprobante adjunto para abrirlo con el cliente de correo.
        /// </summary>
        public string CrearBorradorComprobante(string destinatario, string rutaPdf, string asunto)
        {
            authorizationService.DemandPermission(Permissions.PayslipsEmail);
            if (string.IsNullOrWhiteSpace(destinatario) || string.IsNullOrWhiteSpace(rutaPdf))
            {
                throw new ArgumentException("Debe indicar destinatario y ruta del comprobante.");
            }

            if (!IsValidEmail(destinatario))
            {
                throw new ArgumentException("El correo del destinatario no tiene un formato válido.");
            }

            if (!File.Exists(rutaPdf))
            {
                throw new FileNotFoundException("No se encontró el PDF del comprobante.", rutaPdf);
            }

            string fileName = "EmailComprobante_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".eml";
            string path = PathHelper.RequestExportPath(Path.GetFileNameWithoutExtension(fileName), ".eml", "Mensaje de correo (*.eml)|*.eml");
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            string boundary = "----=_Nomina_" + Guid.NewGuid().ToString("N");
            string subject = string.IsNullOrWhiteSpace(asunto) ? "Comprobante de pago" : asunto.Trim();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("To: " + destinatario.Trim());
            builder.AppendLine("Subject: " + subject);
            builder.AppendLine("X-Unsent: 1");
            builder.AppendLine("MIME-Version: 1.0");
            builder.AppendLine("Content-Type: multipart/mixed; boundary=\"" + boundary + "\"");
            builder.AppendLine();
            builder.AppendLine("--" + boundary);
            builder.AppendLine("Content-Type: text/plain; charset=utf-8");
            builder.AppendLine("Content-Transfer-Encoding: 8bit");
            builder.AppendLine();
            builder.AppendLine("Adjunto encontrará su comprobante de pago generado por el Sistema de Gestión de Nómina.");
            builder.AppendLine();
            builder.AppendLine("--" + boundary);
            builder.AppendLine("Content-Type: application/pdf; name=\"" + Path.GetFileName(rutaPdf) + "\"");
            builder.AppendLine("Content-Transfer-Encoding: base64");
            builder.AppendLine("Content-Disposition: attachment; filename=\"" + Path.GetFileName(rutaPdf) + "\"");
            builder.AppendLine();
            builder.AppendLine(ToBase64Lines(File.ReadAllBytes(rutaPdf)));
            builder.AppendLine("--" + boundary + "--");

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
            auditTrailService.RegistrarAccion("Comprobantes", "Crear borrador de correo", destinatario.Trim());
            return path;
        }

        private static bool IsValidEmail(string email)
        {
            string value = email.Trim();
            int at = value.IndexOf('@');
            return at > 0 && at < value.Length - 3 && value.LastIndexOf('.') > at + 1;
        }

        private static string ToBase64Lines(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < base64.Length; i += 76)
            {
                int length = Math.Min(76, base64.Length - i);
                builder.AppendLine(base64.Substring(i, length));
            }

            return builder.ToString();
        }
    }
}
