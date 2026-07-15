using System;
using System.Collections.Generic;
using SistemaGestionNomina.Security;

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
            permisosPorRol[Roles.Admin] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "*" };
            permisosPorRol[Roles.RRHH] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Permissions.EmployeesView, Permissions.EmployeesCreate, Permissions.EmployeesEdit,
                Permissions.EmployeesDeactivate, Permissions.EmployeesExport,
                Permissions.EmployeeHistoryView, Permissions.EmployeeHistoryExport,
                Permissions.EmployeeChangesSchedule,
                Permissions.AttendanceView, Permissions.AttendanceRegister, Permissions.AttendanceImport,
                Permissions.AttendanceExport, Permissions.PayslipsView, Permissions.PayslipsExport,
                Permissions.PayslipsPrint, Permissions.PayslipsEmail,
                Permissions.ReportsView, Permissions.ReportsPersonal, Permissions.ReportsExport,
                Permissions.PayrollHistoryView,
                Permissions.AbsencesView, Permissions.AbsencesCreate, Permissions.AbsencesApprove,
                Permissions.AbsencesReject, Permissions.AbsencesCancel, Permissions.AbsencesExport
            };
            permisosPorRol[Roles.Contabilidad] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Permissions.PayrollView, Permissions.PayrollCalculate, Permissions.PayrollConfirm,
                Permissions.PayrollExport, Permissions.PayslipsView, Permissions.PayslipsExport,
                Permissions.PayslipsPrint, Permissions.PayslipsEmail,
                Permissions.ReportsView, Permissions.ReportsFinancial, Permissions.ReportsExport,
                Permissions.PayrollPay, Permissions.PayrollAnnul, Permissions.PayrollRecalculate,
                Permissions.PayrollHistoryView, Permissions.AbsencesView
            };
            permisosPorRol[Roles.Supervisor] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Permissions.AttendanceView, Permissions.AttendanceRegister, Permissions.AttendanceExport,
                Permissions.AbsencesView, Permissions.AbsencesApprove, Permissions.AbsencesReject
            };
            permisosPorRol[Roles.Trabajador] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Permissions.PortalView, Permissions.OwnProfileView, Permissions.OwnAttendanceView,
                Permissions.OwnPayslipsView, Permissions.OwnPayslipsDownload, Permissions.OwnPasswordChange,
                Permissions.OwnAbsencesView, Permissions.OwnAbsencesCreate, Permissions.OwnAbsencesCancel
            };

            HashSet<string> admin = permisosPorRol[Roles.Admin];
            admin.Add(Permissions.DashboardView);
        }

        /// <summary>
        /// Indica si un rol tiene permiso para ejecutar una acción.
        /// </summary>
        public bool TienePermiso(string rol, string accion)
        {
            if (string.IsNullOrWhiteSpace(rol) || string.IsNullOrWhiteSpace(accion))
            {
                return false;
            }

            HashSet<string> permisos;
            if (!permisosPorRol.TryGetValue(rol.Trim(), out permisos))
            {
                return false;
            }

            return permisos.Contains("*") || permisos.Contains(accion.Trim());
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

        public bool EsRolValido(string rol)
        {
            return !string.IsNullOrWhiteSpace(rol) && permisosPorRol.ContainsKey(rol.Trim());
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
