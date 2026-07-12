using System;

namespace SistemaGestionNomina.Models
{
    public enum AuthenticationStatus
    {
        Success,
        InvalidCredentials,
        TemporarilyBlocked
    }

    public sealed class AuthenticationResult
    {
        public AuthenticationStatus Status { get; private set; }
        public Usuario User { get; private set; }
        public string Message { get; private set; }
        public DateTime? BlockedUntil { get; private set; }

        public bool IsSuccess
        {
            get { return Status == AuthenticationStatus.Success && User != null; }
        }

        public static AuthenticationResult Successful(Usuario user)
        {
            return new AuthenticationResult
            {
                Status = AuthenticationStatus.Success,
                User = user,
                Message = string.Empty
            };
        }

        public static AuthenticationResult Invalid()
        {
            return new AuthenticationResult
            {
                Status = AuthenticationStatus.InvalidCredentials,
                Message = "Usuario o contraseña incorrectos."
            };
        }

        public static AuthenticationResult Blocked(DateTime blockedUntil)
        {
            return new AuthenticationResult
            {
                Status = AuthenticationStatus.TemporarilyBlocked,
                BlockedUntil = blockedUntil,
                Message = "El acceso está bloqueado temporalmente. Intente nuevamente después de " +
                    blockedUntil.ToString("HH:mm") + "."
            };
        }
    }
}
