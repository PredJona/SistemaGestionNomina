using System;
using System.Data.SQLite;
using System.IO;
using SistemaGestionNomina.Helpers;

namespace SistemaGestionNomina.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            string folder = Path.GetDirectoryName(SQLiteConnectionFactory.DatabasePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            bool existed = File.Exists(SQLiteConnectionFactory.DatabasePath);
            if (!existed)
            {
                SQLiteConnection.CreateFile(SQLiteConnectionFactory.DatabasePath);
            }

            if (existed)
            {
                using (SQLiteConnection versionConnection = SQLiteConnectionFactory.CreateConnection())
                {
                    if (DatabaseMigrationRunner.GetCurrentVersion(versionConnection) < DatabaseMigrationRunner.LatestVersion)
                    {
                        versionConnection.Close();
                        DatabaseMigrationRunner.BackupBeforeMigration(SQLiteConnectionFactory.DatabasePath);
                    }
                }
            }

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                Execute(connection, "PRAGMA foreign_keys = ON;");
                CreateTables(connection);
                DatabaseMigrationRunner.RunPending(connection);
                SeedData(connection);
            }
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            Execute(connection, @"CREATE TABLE IF NOT EXISTS Usuarios (
                IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
                NombreUsuario TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                Rol TEXT NOT NULL,
                Estado TEXT NOT NULL
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Departamentos (
                IdDepartamento INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL UNIQUE
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Empleados (
                IdEmpleado INTEGER PRIMARY KEY AUTOINCREMENT,
                Codigo TEXT NOT NULL UNIQUE,
                Nombre TEXT NOT NULL,
                Apellido TEXT NOT NULL,
                Cedula TEXT NOT NULL UNIQUE,
                Cargo TEXT NOT NULL,
                IdDepartamento INTEGER NOT NULL,
                SalarioBase REAL NOT NULL,
                Estado TEXT NOT NULL,
                FechaIngreso TEXT NOT NULL,
                FOREIGN KEY(IdDepartamento) REFERENCES Departamentos(IdDepartamento)
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Asistencias (
                IdAsistencia INTEGER PRIMARY KEY AUTOINCREMENT,
                IdEmpleado INTEGER NOT NULL,
                Fecha TEXT NOT NULL,
                HoraEntrada TEXT,
                HoraSalida TEXT,
                HorasTrabajadas REAL,
                Estado TEXT NOT NULL,
                FOREIGN KEY(IdEmpleado) REFERENCES Empleados(IdEmpleado)
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS PeriodosNomina (
                IdPeriodo INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                FechaInicio TEXT NOT NULL,
                FechaFin TEXT NOT NULL,
                Estado TEXT NOT NULL
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Nominas (
                IdNomina INTEGER PRIMARY KEY AUTOINCREMENT,
                IdPeriodo INTEGER NOT NULL,
                FechaCalculo TEXT NOT NULL,
                TotalIngresos REAL NOT NULL,
                TotalDeducciones REAL NOT NULL,
                TotalNeto REAL NOT NULL,
                Estado TEXT NOT NULL,
                FOREIGN KEY(IdPeriodo) REFERENCES PeriodosNomina(IdPeriodo)
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS NominaDetalle (
                IdDetalle INTEGER PRIMARY KEY AUTOINCREMENT,
                IdNomina INTEGER NOT NULL,
                IdEmpleado INTEGER NOT NULL,
                SueldoBase REAL NOT NULL,
                Bonos REAL NOT NULL,
                HorasExtra REAL NOT NULL,
                MontoHorasExtra REAL NOT NULL,
                TotalIngresos REAL NOT NULL,
                TotalDeducciones REAL NOT NULL,
                NetoPagar REAL NOT NULL,
                FOREIGN KEY(IdNomina) REFERENCES Nominas(IdNomina),
                FOREIGN KEY(IdEmpleado) REFERENCES Empleados(IdEmpleado)
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Comprobantes (
                IdComprobante INTEGER PRIMARY KEY AUTOINCREMENT,
                IdNomina INTEGER NOT NULL,
                IdEmpleado INTEGER NOT NULL,
                NumeroComprobante TEXT NOT NULL UNIQUE,
                FechaGeneracion TEXT NOT NULL,
                RutaPdf TEXT,
                FOREIGN KEY(IdNomina) REFERENCES Nominas(IdNomina),
                FOREIGN KEY(IdEmpleado) REFERENCES Empleados(IdEmpleado)
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS ConfiguracionNomina (
                IdConfiguracion INTEGER PRIMARY KEY AUTOINCREMENT,
                NombreParametro TEXT NOT NULL UNIQUE,
                Valor REAL NOT NULL,
                Descripcion TEXT
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS ReportesGenerados (
                IdReporte INTEGER PRIMARY KEY AUTOINCREMENT,
                NombreReporte TEXT NOT NULL,
                Tipo TEXT NOT NULL,
                GeneradoPor TEXT NOT NULL,
                FechaGeneracion TEXT NOT NULL,
                RutaArchivo TEXT
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS Auditoria (
                IdAuditoria INTEGER PRIMARY KEY AUTOINCREMENT,
                Usuario TEXT NOT NULL,
                Modulo TEXT NOT NULL,
                Accion TEXT NOT NULL,
                Detalle TEXT,
                Fecha TEXT NOT NULL
            );");

            Execute(connection, @"CREATE TABLE IF NOT EXISTS MigracionesLog (
                IdMigracion INTEGER PRIMARY KEY AUTOINCREMENT,
                Version INTEGER NOT NULL,
                Nombre TEXT NOT NULL,
                FechaAplicacion TEXT NOT NULL,
                Detalle TEXT
            );");
        }

        private static void SeedData(SQLiteConnection connection)
        {
            object adminExists = ExecuteScalar(connection,
                "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @usuario;",
                new SQLiteParameter("@usuario", "admin"));
            if (Convert.ToInt32(adminExists) == 0)
            {
                ExecuteWithParameters(connection,
                    "INSERT INTO Usuarios (NombreUsuario, PasswordHash, Rol, Estado) VALUES (@u, @p, @r, @e);",
                    new SQLiteParameter("@u", "admin"),
                    new SQLiteParameter("@p", PasswordHelper.HashPassword("admin123")),
                    new SQLiteParameter("@r", "Admin"),
                    new SQLiteParameter("@e", "Activo"));
            }

            string[] departments = { "Tecnología", "Ventas", "Recursos Humanos", "Finanzas", "Operaciones" };
            for (int i = 0; i < departments.Length; i++)
            {
                ExecuteWithParameters(connection,
                    "INSERT OR IGNORE INTO Departamentos (Nombre) VALUES (@nombre);",
                    new SQLiteParameter("@nombre", departments[i]));
            }

            SeedEmployee(connection, "EMP-1001", "Carlos", "Mendoza", "8-765-123", "Desarrollador Senior", "Tecnología", 2450);
            SeedEmployee(connection, "EMP-1002", "Ana", "López", "8-432-987", "Ejecutiva de Ventas", "Ventas", 1850);
            SeedEmployee(connection, "EMP-1003", "Sofía", "Gómez", "9-111-222", "Analista de RRHH", "Recursos Humanos", 1700);
            SeedEmployee(connection, "EMP-1004", "Javier", "Ramírez", "4-555-321", "Supervisor de Operaciones", "Operaciones", 2100);

            SeedConfig(connection, "SeguroSocial", 9.75m, "Porcentaje académico de seguro social.");
            SeedConfig(connection, "ISR", 10, "Porcentaje académico de impuesto sobre la renta.");
            SeedConfig(connection, "SeguroEducativo", 1.25m, "Porcentaje académico de seguro educativo.");
            SeedConfig(connection, "RecargoHoraExtra", 1.25m, "Multiplicador académico de horas extra.");
            SeedConfig(connection, "HorasMensualesBase", 160, "Horas mensuales base para cálculo por hora.");
        }

        private static void SeedEmployee(SQLiteConnection connection, string code, string name, string lastName, string cedula, string role, string department, decimal salary)
        {
            object departmentId = ExecuteScalar(connection, "SELECT IdDepartamento FROM Departamentos WHERE Nombre = @nombre;",
                new SQLiteParameter("@nombre", department));

            ExecuteWithParameters(connection, @"INSERT OR IGNORE INTO Empleados
                (Codigo, Nombre, Apellido, Cedula, Cargo, IdDepartamento, SalarioBase, Estado, FechaIngreso)
                VALUES (@codigo, @nombre, @apellido, @cedula, @cargo, @departamento, @salario, 'Activo', @fecha);",
                new SQLiteParameter("@codigo", code),
                new SQLiteParameter("@nombre", name),
                new SQLiteParameter("@apellido", lastName),
                new SQLiteParameter("@cedula", cedula),
                new SQLiteParameter("@cargo", role),
                new SQLiteParameter("@departamento", Convert.ToInt32(departmentId)),
                new SQLiteParameter("@salario", salary),
                new SQLiteParameter("@fecha", DateTime.Today.AddMonths(-8).ToString("yyyy-MM-dd")));
        }

        private static void SeedConfig(SQLiteConnection connection, string name, decimal value, string description)
        {
            ExecuteWithParameters(connection,
                "INSERT OR IGNORE INTO ConfiguracionNomina (NombreParametro, Valor, Descripcion) VALUES (@n, @v, @d);",
                new SQLiteParameter("@n", name),
                new SQLiteParameter("@v", value),
                new SQLiteParameter("@d", description));
        }

        private static void Execute(SQLiteConnection connection, string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void ExecuteWithParameters(SQLiteConnection connection, string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        private static object ExecuteScalar(SQLiteConnection connection, string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
        }
    }
}
