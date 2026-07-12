using System;
using System.IO;
using System.Reflection;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

internal static class Fase2PdfSmokeHost
{
    private static int Main(string[] args)
    {
        if (args.Length != 6) return 2;
        try
        {
            string dbPath = args[0];
            string pdfPath = args[1];
            int ownPayslipId = int.Parse(args[2]);
            int foreignPayslipId = int.Parse(args[3]);
            int userId = int.Parse(args[4]);
            string username = args[5];
            Environment.SetEnvironmentVariable("NOMINA_DB_PATH", dbPath);
            DatabaseInitializer.Initialize();
            SessionContext.Begin(new Usuario
            {
                IdUsuario = userId,
                NombreUsuario = username,
                Rol = Roles.Trabajador,
                Estado = "Activo",
                IdEmpleado = 1
            });

            EmployeePortalService service = new EmployeePortalService();
            MethodInfo download = null;
            MethodInfo[] methods = typeof(EmployeePortalService).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name == "DownloadMyPayslipPdf" && methods[i].GetParameters().Length == 2)
                {
                    download = methods[i];
                    break;
                }
            }

            if (download == null) throw new InvalidOperationException("No se encontro la descarga interna de prueba.");
            string generated = Convert.ToString(download.Invoke(service, new object[] { ownPayslipId, pdfPath }));
            if (!File.Exists(generated)) throw new InvalidOperationException("No se genero el PDF personal.");
            byte[] bytes = File.ReadAllBytes(generated);
            if (bytes.Length < 4 || bytes[0] != 37 || bytes[1] != 80 || bytes[2] != 68 || bytes[3] != 70)
            {
                throw new InvalidOperationException("El archivo generado no tiene cabecera PDF.");
            }

            bool rejected = false;
            try
            {
                download.Invoke(service, new object[] { foreignPayslipId, Path.Combine(Path.GetDirectoryName(pdfPath), "ajeno.pdf") });
            }
            catch (TargetInvocationException ex)
            {
                rejected = ex.InnerException is UnauthorizedAccessException;
            }

            if (!rejected) throw new InvalidOperationException("La descarga permitio un comprobante ajeno.");
            SessionContext.Clear();
            Console.WriteLine("PDF personal y rechazo de archivo ajeno OK");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}
