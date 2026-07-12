using System;
using SistemaGestionNomina.Data;

namespace SistemaGestionNomina.Security
{
    public sealed class EmployeeScopeService
    {
        private readonly EmpleadoRepository employeeRepository = new EmpleadoRepository();

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
