namespace SistemaGestionNomina.Security
{
    public static class Permissions
    {
        public const string DashboardView = "dashboard.ver";
        public const string EmployeesView = "empleados.ver";
        public const string EmployeesCreate = "empleados.crear";
        public const string EmployeesEdit = "empleados.editar";
        public const string EmployeesDeactivate = "empleados.desactivar";
        public const string EmployeesExport = "empleados.exportar";
        public const string AttendanceView = "asistencia.ver";
        public const string AttendanceRegister = "asistencia.registrar";
        public const string AttendanceImport = "asistencia.importar";
        public const string AttendanceExport = "asistencia.exportar";
        public const string PayrollView = "nomina.ver";
        public const string PayrollCalculate = "nomina.calcular";
        public const string PayrollConfirm = "nomina.confirmar";
        public const string PayrollExport = "nomina.exportar";
        public const string PayslipsView = "comprobantes.ver";
        public const string PayslipsExport = "comprobantes.exportar";
        public const string PayslipsPrint = "comprobantes.imprimir";
        public const string PayslipsEmail = "comprobantes.email";
        public const string ReportsView = "reportes.ver";
        public const string ReportsPersonal = "reportes.personal";
        public const string ReportsFinancial = "reportes.financiero";
        public const string ReportsExport = "reportes.exportar";
        public const string ConfigurationView = "configuracion.ver";
        public const string ConfigurationEdit = "configuracion.editar";
        public const string BackupsManage = "backups.gestionar";
        public const string AuditView = "auditoria.ver";
        public const string PortalView = "portal.ver";
        public const string OwnProfileView = "portal.perfil.ver";
        public const string OwnAttendanceView = "portal.asistencia.ver";
        public const string OwnPayslipsView = "portal.comprobantes.ver";
        public const string OwnPayslipsDownload = "portal.comprobantes.descargar";
        public const string OwnPasswordChange = "cuenta.password.cambiar";

        public const string PayrollPay = "nomina.pagar";
        public const string PayrollAnnul = "nomina.anular";
        public const string PayrollRecalculate = "nomina.recalcular";
        public const string PayrollHistoryView = "nomina.historial.ver";
    }
}
