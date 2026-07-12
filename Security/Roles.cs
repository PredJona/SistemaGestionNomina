namespace SistemaGestionNomina.Security
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string RRHH = "RRHH";
        public const string Contabilidad = "Contabilidad";
        public const string Supervisor = "Supervisor";
        public const string Trabajador = "Trabajador";

        public static bool IsValid(string role)
        {
            return role == Admin || role == RRHH || role == Contabilidad ||
                role == Supervisor || role == Trabajador;
        }
    }
}
