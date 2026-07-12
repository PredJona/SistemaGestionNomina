using System;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SistemaGestionNomina.Data
{
    public static class DatabaseMigrationRunner
    {
        public const int LatestVersion = 3;

        public static int GetCurrentVersion(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand("PRAGMA user_version;", connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static string BackupBeforeMigration(string databasePath)
        {
            string folder = Path.Combine(Path.GetDirectoryName(databasePath), "Backups", "Migrations");
            Directory.CreateDirectory(folder);
            string destination = Path.Combine(folder,
                "nomina_pre_migracion_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".db");
            File.Copy(databasePath, destination, false);

            string hash = ComputeSha256(destination);
            File.WriteAllText(destination + ".sha256", hash, Encoding.ASCII);
            return destination;
        }

        public static void RunPending(SQLiteConnection connection)
        {
            int version = GetCurrentVersion(connection);
            if (version < 1) ApplyMigration(connection, 1, "Seguridad de usuarios", MigrateUsers);
            if (version < 2) ApplyMigration(connection, 2, "Índices e integridad de asistencia", MigrateIndexesAndAttendance);
            if (version < 3) ApplyMigration(connection, 3, "Validaciones críticas", MigrateIntegrityRules);
        }

        private static void ApplyMigration(SQLiteConnection connection, int version, string name,
            Action<SQLiteConnection, SQLiteTransaction, StringBuilder> migration)
        {
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                StringBuilder detail = new StringBuilder();
                migration(connection, transaction, detail);
                Execute(connection, transaction, "PRAGMA user_version = " + version + ";");
                Execute(connection, transaction, @"INSERT INTO MigracionesLog
                    (Version, Nombre, FechaAplicacion, Detalle)
                    VALUES (@version, @nombre, @fecha, @detalle);",
                    new SQLiteParameter("@version", version),
                    new SQLiteParameter("@nombre", name),
                    new SQLiteParameter("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SQLiteParameter("@detalle", detail.ToString()));
                transaction.Commit();
            }
        }

        private static void MigrateUsers(SQLiteConnection connection, SQLiteTransaction transaction, StringBuilder detail)
        {
            AddColumnIfMissing(connection, transaction, "Usuarios", "IdEmpleado",
                "INTEGER NULL REFERENCES Empleados(IdEmpleado)");
            AddColumnIfMissing(connection, transaction, "Usuarios", "UltimoAcceso", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Usuarios", "IntentosFallidos", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, transaction, "Usuarios", "Bloqueado", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, transaction, "Usuarios", "FechaBloqueo", "TEXT NULL");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_Usuarios_IdEmpleado ON Usuarios(IdEmpleado);");
            detail.Append("Columnas de autenticación e índice de empleado agregados.");
        }

        private static void MigrateIndexesAndAttendance(SQLiteConnection connection, SQLiteTransaction transaction, StringBuilder detail)
        {
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Empleados_Codigo ON Empleados(Codigo);");
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Empleados_Cedula ON Empleados(Cedula);");
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Asistencias_EmpleadoFecha ON Asistencias(IdEmpleado, Fecha);");
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Nominas_IdPeriodo ON Nominas(IdPeriodo);");
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Comprobantes_IdEmpleado ON Comprobantes(IdEmpleado);");
            Execute(connection, transaction, "CREATE INDEX IF NOT EXISTS IX_Auditoria_Fecha ON Auditoria(Fecha DESC);");

            int duplicateGroups = Convert.ToInt32(Scalar(connection, transaction, @"SELECT COUNT(1) FROM (
                SELECT IdEmpleado, Fecha FROM Asistencias
                GROUP BY IdEmpleado, Fecha HAVING COUNT(1) > 1);"));

            if (duplicateGroups == 0)
            {
                Execute(connection, transaction,
                    "CREATE UNIQUE INDEX IF NOT EXISTS UX_Asistencias_EmpleadoFecha ON Asistencias(IdEmpleado, Fecha);");
                detail.Append("No se detectaron asistencias duplicadas; se creó el índice único. ");
            }
            else
            {
                detail.Append("Se detectaron " + duplicateGroups +
                    " grupos históricos duplicados; se conservaron sin cambios. ");
            }

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Asistencias_NoDuplicar_Insert
                BEFORE INSERT ON Asistencias
                WHEN EXISTS (SELECT 1 FROM Asistencias
                    WHERE IdEmpleado = NEW.IdEmpleado AND Fecha = NEW.Fecha)
                BEGIN
                    SELECT RAISE(ABORT, 'Ya existe una asistencia para el empleado en esa fecha.');
                END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Asistencias_NoDuplicar_Update
                BEFORE UPDATE OF IdEmpleado, Fecha ON Asistencias
                WHEN EXISTS (SELECT 1 FROM Asistencias
                    WHERE IdEmpleado = NEW.IdEmpleado AND Fecha = NEW.Fecha
                      AND IdAsistencia <> OLD.IdAsistencia)
                BEGIN
                    SELECT RAISE(ABORT, 'Ya existe una asistencia para el empleado en esa fecha.');
                END;");
            detail.Append("Se instalaron protecciones para impedir nuevas duplicaciones.");
        }

        private static void MigrateIntegrityRules(SQLiteConnection connection, SQLiteTransaction transaction, StringBuilder detail)
        {
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Usuarios_Validar_Insert
                BEFORE INSERT ON Usuarios
                WHEN NEW.Rol NOT IN ('Admin','RRHH','Contabilidad','Supervisor','Trabajador')
                  OR (NEW.Rol IN ('Supervisor','Trabajador') AND NEW.IdEmpleado IS NULL)
                BEGIN
                    SELECT RAISE(ABORT, 'El rol o la asociación del usuario no es válida.');
                END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Usuarios_Validar_Update
                BEFORE UPDATE OF Rol, IdEmpleado ON Usuarios
                WHEN NEW.Rol NOT IN ('Admin','RRHH','Contabilidad','Supervisor','Trabajador')
                  OR (NEW.Rol IN ('Supervisor','Trabajador') AND NEW.IdEmpleado IS NULL)
                BEGIN
                    SELECT RAISE(ABORT, 'El rol o la asociación del usuario no es válida.');
                END;");
            int duplicateUserLinks = Convert.ToInt32(Scalar(connection, transaction, @"SELECT COUNT(1) FROM (
                SELECT IdEmpleado FROM Usuarios WHERE IdEmpleado IS NOT NULL
                GROUP BY IdEmpleado HAVING COUNT(1) > 1);"));
            if (duplicateUserLinks == 0)
            {
                Execute(connection, transaction, @"CREATE UNIQUE INDEX IF NOT EXISTS UX_Usuarios_IdEmpleado
                    ON Usuarios(IdEmpleado) WHERE IdEmpleado IS NOT NULL;");
            }
            else
            {
                detail.Append("Se conservaron " + duplicateUserLinks + " asociaciones históricas duplicadas. ");
            }
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Usuarios_NoDuplicarEmpleado_Insert
                BEFORE INSERT ON Usuarios
                WHEN NEW.IdEmpleado IS NOT NULL AND EXISTS
                    (SELECT 1 FROM Usuarios WHERE IdEmpleado = NEW.IdEmpleado)
                BEGIN
                    SELECT RAISE(ABORT, 'El empleado ya tiene un usuario asociado.');
                END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Usuarios_NoDuplicarEmpleado_Update
                BEFORE UPDATE OF IdEmpleado ON Usuarios
                WHEN NEW.IdEmpleado IS NOT NULL AND EXISTS
                    (SELECT 1 FROM Usuarios WHERE IdEmpleado = NEW.IdEmpleado AND IdUsuario <> OLD.IdUsuario)
                BEGIN
                    SELECT RAISE(ABORT, 'El empleado ya tiene un usuario asociado.');
                END;");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Empleados_Validar_Insert
                BEFORE INSERT ON Empleados
                WHEN NEW.SalarioBase <= 0 OR NEW.IdDepartamento <= 0
                BEGIN
                    SELECT RAISE(ABORT, 'El salario y el departamento del empleado deben ser válidos.');
                END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Empleados_Validar_Update
                BEFORE UPDATE OF SalarioBase, IdDepartamento ON Empleados
                WHEN NEW.SalarioBase <= 0 OR NEW.IdDepartamento <= 0
                BEGIN
                    SELECT RAISE(ABORT, 'El salario y el departamento del empleado deben ser válidos.');
                END;");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Asistencias_Validar_Insert
                BEFORE INSERT ON Asistencias
                WHEN NEW.HorasTrabajadas < 0
                  OR (NEW.HoraEntrada IS NOT NULL AND NEW.HoraSalida IS NOT NULL AND NEW.HoraSalida <= NEW.HoraEntrada)
                BEGIN
                    SELECT RAISE(ABORT, 'Las horas de asistencia no son válidas.');
                END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Asistencias_Validar_Update
                BEFORE UPDATE OF HoraEntrada, HoraSalida, HorasTrabajadas ON Asistencias
                WHEN NEW.HorasTrabajadas < 0
                  OR (NEW.HoraEntrada IS NOT NULL AND NEW.HoraSalida IS NOT NULL AND NEW.HoraSalida <= NEW.HoraEntrada)
                BEGIN
                    SELECT RAISE(ABORT, 'Las horas de asistencia no son válidas.');
                END;");
            detail.Append("Roles, asociaciones, salarios, departamentos y horas protegidos mediante índices y triggers.");
        }

        private static void AddColumnIfMissing(SQLiteConnection connection, SQLiteTransaction transaction,
            string table, string column, string definition)
        {
            if (ColumnExists(connection, transaction, table, column)) return;
            Execute(connection, transaction, "ALTER TABLE " + table + " ADD COLUMN " + column + " " + definition + ";");
        }

        private static bool ColumnExists(SQLiteConnection connection, SQLiteTransaction transaction, string table, string column)
        {
            using (SQLiteCommand command = new SQLiteCommand("PRAGMA table_info(" + table + ");", connection, transaction))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (string.Equals(Convert.ToString(reader["name"]), column, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static object Scalar(SQLiteConnection connection, SQLiteTransaction transaction, string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection, transaction))
            {
                return command.ExecuteScalar();
            }
        }

        private static void Execute(SQLiteConnection connection, SQLiteTransaction transaction, string sql,
            params SQLiteParameter[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection, transaction))
            {
                if (parameters != null && parameters.Length > 0) command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        private static string ComputeSha256(string path)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream stream = File.OpenRead(path))
            {
                byte[] hash = sha.ComputeHash(stream);
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++) builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                return builder.ToString();
            }
        }
    }
}
