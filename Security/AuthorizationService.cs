using System;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.Security
{
    public sealed class AuthorizationService
    {
        private readonly RolePermissionService rolePermissionService = new RolePermissionService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        public bool HasPermission(string permission)
        {
            return SessionContext.IsAuthenticated &&
                rolePermissionService.TienePermiso(SessionContext.Role, permission);
        }

        public void DemandPermission(string permission)
        {
            if (HasPermission(permission)) return;
            auditTrailService.RegistrarAccion("Seguridad", "Acceso rechazado", permission ?? string.Empty);
            throw new UnauthorizedAccessException("No tiene permisos para realizar esta operación.");
        }

        public void DemandAny(params string[] permissions)
        {
            if (permissions != null)
            {
                for (int i = 0; i < permissions.Length; i++)
                {
                    if (HasPermission(permissions[i])) return;
                }
            }

            auditTrailService.RegistrarAccion("Seguridad", "Acceso rechazado",
                permissions == null ? string.Empty : string.Join(", ", permissions));
            throw new UnauthorizedAccessException("No tiene permisos para realizar esta operación.");
        }
    }
}
