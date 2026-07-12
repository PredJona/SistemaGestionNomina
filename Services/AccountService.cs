using System;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public sealed class AccountService
    {
        private readonly AuthenticationRepository authenticationRepository = new AuthenticationRepository();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly EmployeeScopeService employeeScopeService = new EmployeeScopeService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public void ChangeOwnPassword(string currentPassword, string newPassword, string confirmation)
        {
            authorizationService.DemandPermission(Permissions.OwnPasswordChange);
            employeeScopeService.RequireCurrentEmployeeId();

            Usuario sessionUser = SessionContext.CurrentUser;
            if (sessionUser == null)
            {
                throw new UnauthorizedAccessException("No existe una sesion autenticada.");
            }

            Usuario storedUser = authenticationRepository.GetByUsername(SessionContext.Username);
            if (storedUser == null || storedUser.IdUsuario != sessionUser.IdUsuario)
            {
                throw new UnauthorizedAccessException("No se pudo validar la cuenta de la sesion.");
            }

            if (!PasswordHelper.Verify(currentPassword, storedUser.PasswordHash))
            {
                AuditRejected("Contrasena actual invalida");
                throw new ArgumentException("La contrasena actual no es correcta.");
            }

            ValidateNewPassword(newPassword, confirmation, storedUser.PasswordHash);
            string newHash = PasswordHelper.HashPassword(newPassword);
            if (!authenticationRepository.UpdatePassword(storedUser.IdUsuario, newHash))
            {
                throw new InvalidOperationException("No se pudo actualizar la contrasena.");
            }

            sessionUser.PasswordHash = newHash;
            auditTrailService.RegistrarAccion("Cuenta", "Cambiar contrasena", "Actualizacion completada");
        }

        private void ValidateNewPassword(string newPassword, string confirmation, string currentHash)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                AuditRejected("Nueva contrasena vacia");
                throw new ArgumentException("La nueva contrasena es obligatoria.");
            }

            if (!string.Equals(newPassword, confirmation, StringComparison.Ordinal))
            {
                AuditRejected("Confirmacion diferente");
                throw new ArgumentException("La confirmacion no coincide con la nueva contrasena.");
            }

            if (PasswordHelper.Verify(newPassword, currentHash))
            {
                AuditRejected("Contrasena reutilizada");
                throw new ArgumentException("La nueva contrasena debe ser diferente de la actual.");
            }

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            for (int i = 0; i < newPassword.Length; i++)
            {
                hasUpper |= char.IsUpper(newPassword[i]);
                hasLower |= char.IsLower(newPassword[i]);
                hasDigit |= char.IsDigit(newPassword[i]);
            }

            if (newPassword.Length < 8 || !hasUpper || !hasLower || !hasDigit)
            {
                AuditRejected("Politica de contrasena incumplida");
                throw new ArgumentException("Use al menos 8 caracteres, con mayuscula, minuscula y numero.");
            }
        }

        private void AuditRejected(string reason)
        {
            auditTrailService.RegistrarAccion("Cuenta", "Cambio de contrasena rechazado", reason);
        }
    }
}
