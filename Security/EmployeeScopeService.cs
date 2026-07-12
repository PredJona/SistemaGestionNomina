using System;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Security
{
    public sealed class EmployeeScopeService
    {
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();
        private readonly AuditRepository auditRepository = new AuditRepository();

        public int RequireCurrentEmployeeId()
        {
            if (!SessionContext.IsAuthenticated ||
                !string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase))
            {
                throw Reject("Esta operacion requiere una sesion de trabajador.", "Rol o sesion no validos");
            }

            if (!SessionContext.EmployeeId.HasValue || SessionContext.EmployeeId.Value <= 0)
            {
                throw Reject("El usuario no tiene un empleado asociado.", "Cuenta sin empleado asociado");
            }

            Empleado employee = employeeRepository.GetById(SessionContext.EmployeeId.Value);
            if (employee == null)
            {
                throw Reject("No se encontro el empleado asociado a la cuenta.", "Empleado asociado inexistente");
            }

            if (!string.Equals(employee.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
            {
                throw Reject("El empleado asociado no se encuentra activo.", "Empleado asociado inactivo");
            }

            return employee.IdEmpleado;
        }

        public void DemandCurrentEmployee(int employeeId)
        {
            int currentEmployeeId = RequireCurrentEmployeeId();
            if (employeeId <= 0 || employeeId != currentEmployeeId)
            {
                throw Reject("Solo puede consultar su propia informacion laboral.", "Intento de acceso a otro empleado");
            }
        }

        private UnauthorizedAccessException Reject(string message, string detail)
        {
            auditRepository.Add(new AuditRecord
            {
                Usuario = SessionContext.IsAuthenticated ? SessionContext.Username : "anonimo",
                Modulo = "Portal trabajador",
                Accion = "Acceso rechazado",
                Detalle = detail,
                Fecha = DateTime.Now
            });
            return new UnauthorizedAccessException(message);
        }

        public int? GetDepartmentScope()
        {
            if (!string.Equals(SessionContext.Role, Roles.Supervisor, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (!SessionContext.EmployeeId.HasValue)
            {
                throw new UnauthorizedAccessException("El supervisor no tiene un empleado asociado.");
            }

            int? departmentId = employeeRepository.GetDepartmentId(SessionContext.EmployeeId.Value);
            if (!departmentId.HasValue)
            {
                throw new UnauthorizedAccessException("No se encontró el departamento asociado al supervisor.");
            }

            return departmentId;
        }

        public void DemandEmployeeInScope(int employeeId)
        {
            int? scopeDepartmentId = GetDepartmentScope();
            if (!scopeDepartmentId.HasValue) return;

            int? employeeDepartmentId = employeeRepository.GetDepartmentId(employeeId);
            if (!employeeDepartmentId.HasValue || employeeDepartmentId.Value != scopeDepartmentId.Value)
            {
                throw new UnauthorizedAccessException("No puede consultar o modificar empleados de otro departamento.");
            }
        }
    }
}
