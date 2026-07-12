using System;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Security
{
    public sealed class EmployeeScopeService
    {
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();

        public int RequireCurrentEmployeeId()
        {
            if (!SessionContext.IsAuthenticated ||
                !string.Equals(SessionContext.Role, Roles.Trabajador, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Esta operacion requiere una sesion de trabajador.");
            }

            if (!SessionContext.EmployeeId.HasValue || SessionContext.EmployeeId.Value <= 0)
            {
                throw new UnauthorizedAccessException("El usuario no tiene un empleado asociado.");
            }

            Empleado employee = employeeRepository.GetById(SessionContext.EmployeeId.Value);
            if (employee == null)
            {
                throw new UnauthorizedAccessException("No se encontro el empleado asociado a la cuenta.");
            }

            if (!string.Equals(employee.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("El empleado asociado no se encuentra activo.");
            }

            return employee.IdEmpleado;
        }

        public void DemandCurrentEmployee(int employeeId)
        {
            int currentEmployeeId = RequireCurrentEmployeeId();
            if (employeeId <= 0 || employeeId != currentEmployeeId)
            {
                throw new UnauthorizedAccessException("Solo puede consultar su propia informacion laboral.");
            }
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
