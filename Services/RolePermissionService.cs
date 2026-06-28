using System;
using System.Collections.Generic;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio reservado para permisos avanzados por rol.
    /// </summary>
    public class RolePermissionService
    {
        private readonly Dictionary<string, HashSet<string>> permisosPorRol;

        public RolePermissionService()
        {
            permisosPorRol = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            permisosPorRol["Admin"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "*" };
            permisosPorRol["RRHH"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "empleados.ver", "empleados.crear", "empleados.editar", "empleados.desactivar",
                "asistencia.ver", "asistencia.crear", "asistencia.importar",
                "reportes.ver", "reportes.exportar", "comprobantes.ver", "comprobantes.exportar"
            };
            permisosPorRol["Contabilidad"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "nomina.ver", "nomina.calcular", "nomina.confirmar",
                "comprobantes.ver", "comprobantes.exportar", "comprobantes.imprimir", "comprobantes.email",
                "reportes.ver", "reportes.exportar", "configuracion.ver"
            };
            permisosPorRol["Consulta"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "dashboard.ver", "empleados.ver", "asistencia.ver", "nomina.ver", "comprobantes.ver", "reportes.ver"
            };
        }

        /// <summary>
        /// Indica si un rol tiene permiso para ejecutar una acción.
        /// </summary>
        public bool TienePermiso(string rol, string accion)
        {
            if (string.IsNullOrWhiteSpace(accion))
            {
                return false;
            }

            string[] parts = accion.Split('.');
            if (parts.Length == 2)
            {
                return TienePermiso(rol, parts[0], parts[1]);
            }

            return TienePermiso(rol, "*", accion);
        }

        /// <summary>
        /// Indica si un rol puede ejecutar una accion especifica en un modulo.
        /// </summary>
        public bool TienePermiso(string rol, string modulo, string accion)
        {
            if (string.IsNullOrWhiteSpace(rol))
            {
                return false;
            }

            HashSet<string> permisos;
            if (!permisosPorRol.TryGetValue(rol.Trim(), out permisos))
            {
                return false;
            }

            if (permisos.Contains("*"))
            {
                return true;
            }

            string key = (modulo ?? string.Empty).Trim().ToLowerInvariant() + "." + (accion ?? string.Empty).Trim().ToLowerInvariant();
            return permisos.Contains(key);
        }

        /// <summary>
        /// Devuelve las acciones permitidas para mostrar u ocultar opciones de interfaz.
        /// </summary>
        public List<string> ObtenerPermisos(string rol)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(rol))
            {
                return result;
            }

            HashSet<string> permisos;
            if (permisosPorRol.TryGetValue(rol.Trim(), out permisos))
            {
                foreach (string permiso in permisos)
                {
                    result.Add(permiso);
                }
            }

            return result;
        }
    }
}
