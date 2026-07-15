using System;
using System.Windows.Forms;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.UI;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DatabaseInitializer.Initialize();
                new EmployeeHistoryService().ApplyDueChanges(DateTime.Today);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo inicializar la base de datos.\n\n" + ex.Message,
                    "Error de inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool continueLogin = true;
            while (continueLogin)
            {
                using (FrmLogin login = new FrmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }

                    using (FrmMain main = new FrmMain(login.AuthenticatedUser))
                    {
                        Application.Run(main);
                        continueLogin = main.LogoutRequested;
                        if (SessionContext.IsAuthenticated)
                        {
                            new AuditTrailService().RegistrarAccion("Seguridad", "Cerrar aplicación", string.Empty);
                            SessionContext.Clear();
                        }
                    }
                }
            }
        }
    }
}
