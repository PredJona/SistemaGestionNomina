using System;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Security
{
    public static class SessionContext
    {
        private static readonly object Sync = new object();
        private static Usuario currentUser;

        public static Usuario CurrentUser
        {
            get { lock (Sync) { return currentUser; } }
        }

        public static bool IsAuthenticated
        {
            get { return CurrentUser != null; }
        }

        public static string Username
        {
            get { return CurrentUser == null ? string.Empty : CurrentUser.NombreUsuario; }
        }

        public static string Role
        {
            get { return CurrentUser == null ? string.Empty : CurrentUser.Rol; }
        }

        public static int? EmployeeId
        {
            get { return CurrentUser == null ? null : CurrentUser.IdEmpleado; }
        }

        public static void Begin(Usuario user)
        {
            if (user == null || user.IdUsuario <= 0 || string.IsNullOrWhiteSpace(user.NombreUsuario))
            {
                throw new ArgumentException("El usuario autenticado no es válido.", "user");
            }

            lock (Sync)
            {
                currentUser = user;
            }
        }

        public static void Clear()
        {
            lock (Sync)
            {
                currentUser = null;
            }
        }
    }
}
