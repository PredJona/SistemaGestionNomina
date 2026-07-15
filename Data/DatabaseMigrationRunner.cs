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
        public const int LatestVersion = 6;

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
            if (version < 4) ApplyMigration(connection, 4, "Cierre, pago y anulación de nómina", MigrateNominaRobusta);
            if (version < 5) ApplyMigration(connection, 5, "Historial laboral y protección histórica", MigrateEmployeeHistoryAndPayrollProtection);
            if (version < 6) ApplyMigration(connection, 6, "Solicitudes de ausencia", MigrateAbsenceRequests);
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

        private static void MigrateNominaRobusta(SQLiteConnection connection, SQLiteTransaction transaction, StringBuilder detail)
        {
            AddColumnIfMissing(connection, transaction, "PeriodosNomina", "Cerrado",
                "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, transaction, "PeriodosNomina", "FechaCierre", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "PeriodosNomina", "CerradoPor", "TEXT NULL");

            AddColumnIfMissing(connection, transaction, "Nominas", "FechaConfirmacion", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "ConfirmadaPor", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "FechaPago", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "PagadaPor", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "FechaAnulacion", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "AnuladaPor", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "Nominas", "MotivoAnulacion", "TEXT NULL");

            Execute(connection, transaction, @"CREATE TABLE IF NOT EXISTS NominaVersiones (
                IdVersion INTEGER PRIMARY KEY AUTOINCREMENT,
                IdNominaOriginal INTEGER NOT NULL,
                IdNominaNueva INTEGER NULL,
                MotivoCambio TEXT NOT NULL,
                UsuarioResponsable TEXT NOT NULL,
                FechaCambio TEXT NOT NULL,
                FOREIGN KEY(IdNominaOriginal) REFERENCES Nominas(IdNomina),
                FOREIGN KEY(IdNominaNueva) REFERENCES Nominas(IdNomina)
            );");

            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_Nominas_IdPeriodo_Estado ON Nominas(IdPeriodo, Estado);");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_NominaVersiones_IdNominaOriginal ON NominaVersiones(IdNominaOriginal);");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Asistencias_PeriodoCerrado_Insert
                BEFORE INSERT ON Asistencias
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1
                      AND FechaInicio <= NEW.Fecha
                      AND FechaFin >= NEW.Fecha)
                BEGIN
                    SELECT RAISE(ABORT, 'No se puede registrar asistencia en un período cerrado.');
                END;");

            detail.Append("PeriodosNomina (Cerrado,FechaCierre,CerradoPor), Nominas (fechas/usuarios de cada estado), ");
            detail.Append("NominaVersiones, índices y trigger de bloqueo de asistencia creados.");
        }

        private static void MigrateEmployeeHistoryAndPayrollProtection(SQLiteConnection connection,
            SQLiteTransaction transaction, StringBuilder detail)
        {
            AddColumnIfMissing(connection, transaction, "Empleados", "FechaEfectivaLaboral", "TEXT NULL");
            Execute(connection, transaction, @"UPDATE Empleados
                SET FechaEfectivaLaboral = FechaIngreso
                WHERE FechaEfectivaLaboral IS NULL OR FechaEfectivaLaboral = '';" );

            AddColumnIfMissing(connection, transaction, "NominaDetalle", "CodigoEmpleadoSnapshot", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "NominaDetalle", "NombreEmpleadoSnapshot", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "NominaDetalle", "CargoEmpleadoSnapshot", "TEXT NULL");
            AddColumnIfMissing(connection, transaction, "NominaDetalle", "DepartamentoSnapshot", "TEXT NULL");
            Execute(connection, transaction, @"UPDATE NominaDetalle
                SET CodigoEmpleadoSnapshot = COALESCE(CodigoEmpleadoSnapshot,
                        (SELECT Codigo FROM Empleados WHERE IdEmpleado = NominaDetalle.IdEmpleado)),
                    NombreEmpleadoSnapshot = COALESCE(NombreEmpleadoSnapshot,
                        (SELECT Nombre || ' ' || Apellido FROM Empleados WHERE IdEmpleado = NominaDetalle.IdEmpleado)),
                    CargoEmpleadoSnapshot = COALESCE(CargoEmpleadoSnapshot,
                        (SELECT Cargo FROM Empleados WHERE IdEmpleado = NominaDetalle.IdEmpleado)),
                    DepartamentoSnapshot = COALESCE(DepartamentoSnapshot,
                        (SELECT d.Nombre FROM Empleados e INNER JOIN Departamentos d
                         ON d.IdDepartamento = e.IdDepartamento WHERE e.IdEmpleado = NominaDetalle.IdEmpleado));");

            Execute(connection, transaction, @"CREATE TABLE IF NOT EXISTS HistorialEmpleado (
                IdHistorial INTEGER PRIMARY KEY AUTOINCREMENT,
                IdEmpleado INTEGER NOT NULL,
                CampoModificado TEXT NOT NULL,
                ValorAnterior TEXT NULL,
                ValorNuevo TEXT NULL,
                ValorAnteriorTecnico TEXT NULL,
                ValorNuevoTecnico TEXT NULL,
                FechaCambio TEXT NOT NULL,
                FechaEfectiva TEXT NOT NULL,
                UsuarioResponsable TEXT NOT NULL,
                Motivo TEXT NULL,
                Aplicado INTEGER NOT NULL DEFAULT 1,
                FechaAplicacion TEXT NULL,
                FOREIGN KEY(IdEmpleado) REFERENCES Empleados(IdEmpleado)
            );");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_HistorialEmpleado_EmpleadoFecha ON HistorialEmpleado(IdEmpleado, FechaEfectiva DESC);");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_HistorialEmpleado_Pendientes ON HistorialEmpleado(Aplicado, FechaEfectiva);");

            Execute(connection, transaction, "DROP TRIGGER IF EXISTS TR_Asistencias_PeriodoCerrado_Insert;");
            Execute(connection, transaction, @"CREATE TRIGGER TR_Asistencias_PeriodoCerrado_Insert
                BEFORE INSERT ON Asistencias
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1 AND FechaInicio <= NEW.Fecha AND FechaFin >= NEW.Fecha)
                BEGIN
                    SELECT RAISE(ABORT, 'No se puede registrar asistencia en un periodo cerrado.');
                END;");
            Execute(connection, transaction, "DROP TRIGGER IF EXISTS TR_Asistencias_PeriodoCerrado_Update;");
            Execute(connection, transaction, @"CREATE TRIGGER TR_Asistencias_PeriodoCerrado_Update
                BEFORE UPDATE ON Asistencias
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1
                      AND ((FechaInicio <= OLD.Fecha AND FechaFin >= OLD.Fecha)
                        OR (FechaInicio <= NEW.Fecha AND FechaFin >= NEW.Fecha)))
                BEGIN
                    SELECT RAISE(ABORT, 'No se puede modificar asistencia relacionada con un periodo cerrado.');
                END;");
            Execute(connection, transaction, "DROP TRIGGER IF EXISTS TR_Asistencias_PeriodoCerrado_Delete;");
            Execute(connection, transaction, @"CREATE TRIGGER TR_Asistencias_PeriodoCerrado_Delete
                BEFORE DELETE ON Asistencias
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1 AND FechaInicio <= OLD.Fecha AND FechaFin >= OLD.Fecha)
                BEGIN
                    SELECT RAISE(ABORT, 'No se puede eliminar asistencia relacionada con un periodo cerrado.');
                END;");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_PeriodosNomina_Rango_Insert
                BEFORE INSERT ON PeriodosNomina WHEN NEW.FechaInicio > NEW.FechaFin
                BEGIN SELECT RAISE(ABORT, 'La fecha inicial del periodo no puede superar la fecha final.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_PeriodosNomina_Rango_Update
                BEFORE UPDATE OF FechaInicio, FechaFin ON PeriodosNomina WHEN NEW.FechaInicio > NEW.FechaFin
                BEGIN SELECT RAISE(ABORT, 'La fecha inicial del periodo no puede superar la fecha final.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_PeriodosNomina_Solapado_Insert
                BEFORE INSERT ON PeriodosNomina
                WHEN (NEW.Cerrado = 1 OR NEW.Estado IN ('Confirmado','Pagado'))
                  AND EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE (Cerrado = 1 OR Estado IN ('Confirmado','Pagado'))
                      AND FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)
                BEGIN SELECT RAISE(ABORT, 'El periodo se solapa con otro periodo cerrado o confirmado.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_PeriodosNomina_Solapado_Update
                BEFORE UPDATE OF FechaInicio, FechaFin, Estado, Cerrado ON PeriodosNomina
                WHEN (NEW.Cerrado = 1 OR NEW.Estado IN ('Confirmado','Pagado'))
                  AND EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE IdPeriodo <> OLD.IdPeriodo
                      AND (Cerrado = 1 OR Estado IN ('Confirmado','Pagado'))
                      AND FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)
                BEGIN SELECT RAISE(ABORT, 'El periodo se solapa con otro periodo cerrado o confirmado.'); END;");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_HistorialEmpleado_PeriodoCerrado_Insert
                BEFORE INSERT ON HistorialEmpleado
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1 AND FechaFin >= NEW.FechaEfectiva)
                BEGIN SELECT RAISE(ABORT, 'El cambio laboral afectaria un periodo de nomina cerrado.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Empleados_CambioLaboral_PeriodoCerrado
                BEFORE UPDATE OF SalarioBase, Cargo, IdDepartamento, Estado ON Empleados
                WHEN (OLD.SalarioBase <> NEW.SalarioBase OR OLD.Cargo <> NEW.Cargo
                      OR OLD.IdDepartamento <> NEW.IdDepartamento OR OLD.Estado <> NEW.Estado)
                  AND (NEW.FechaEfectivaLaboral IS NULL OR EXISTS (SELECT 1 FROM PeriodosNomina
                    WHERE Cerrado = 1 AND FechaFin >= NEW.FechaEfectivaLaboral))
                BEGIN SELECT RAISE(ABORT, 'El cambio laboral afectaria un periodo de nomina cerrado.'); END;");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Nominas_Estado_Insert
                BEFORE INSERT ON Nominas
                WHEN NEW.Estado NOT IN ('Borrador','Calculada','Confirmada','Pagada','Anulada')
                BEGIN SELECT RAISE(ABORT, 'El estado de nomina no es valido.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Nominas_Estado_Update
                BEFORE UPDATE OF Estado ON Nominas
                WHEN OLD.Estado <> NEW.Estado AND NOT (
                    (OLD.Estado = 'Borrador' AND NEW.Estado = 'Calculada') OR
                    (OLD.Estado = 'Calculada' AND NEW.Estado = 'Confirmada') OR
                    (OLD.Estado = 'Confirmada' AND NEW.Estado IN ('Pagada','Anulada')) OR
                    (OLD.Estado = 'Pagada' AND NEW.Estado = 'Anulada'))
                BEGIN SELECT RAISE(ABORT, 'La transicion de estado de nomina no es valida.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_Nominas_ProtegerDatos_Update
                BEFORE UPDATE OF IdPeriodo, TotalIngresos, TotalDeducciones, TotalNeto ON Nominas
                WHEN OLD.Estado IN ('Confirmada','Pagada','Anulada') AND
                    (OLD.IdPeriodo <> NEW.IdPeriodo OR OLD.TotalIngresos <> NEW.TotalIngresos
                     OR OLD.TotalDeducciones <> NEW.TotalDeducciones OR OLD.TotalNeto <> NEW.TotalNeto)
                BEGIN SELECT RAISE(ABORT, 'Una nomina confirmada, pagada o anulada no puede editarse.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_NominaDetalle_Proteger_Update
                BEFORE UPDATE ON NominaDetalle
                WHEN EXISTS (SELECT 1 FROM Nominas WHERE IdNomina = OLD.IdNomina
                    AND Estado IN ('Confirmada','Pagada','Anulada'))
                BEGIN SELECT RAISE(ABORT, 'El detalle de una nomina cerrada no puede editarse.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_NominaDetalle_Proteger_Delete
                BEFORE DELETE ON NominaDetalle
                WHEN EXISTS (SELECT 1 FROM Nominas WHERE IdNomina = OLD.IdNomina
                    AND Estado IN ('Confirmada','Pagada','Anulada'))
                BEGIN SELECT RAISE(ABORT, 'El detalle de una nomina cerrada no puede eliminarse.'); END;");

            detail.Append("Historial laboral, cambios efectivos, snapshots, bloqueos por rango y estados de nomina instalados.");
        }

        private static void MigrateAbsenceRequests(SQLiteConnection connection,
            SQLiteTransaction transaction, StringBuilder detail)
        {
            Execute(connection, transaction, @"CREATE TABLE IF NOT EXISTS SolicitudesAusencia (
                IdSolicitud INTEGER PRIMARY KEY AUTOINCREMENT,
                IdEmpleado INTEGER NOT NULL,
                Tipo TEXT NOT NULL CHECK (Tipo IN ('Vacaciones','Permiso remunerado','Permiso no remunerado',
                    'Incapacidad médica','Licencia','Ausencia justificada','Ausencia injustificada','Suspensión')),
                FechaInicio TEXT NOT NULL,
                FechaFin TEXT NOT NULL,
                Estado TEXT NOT NULL CHECK (Estado IN ('Pendiente','Aprobada','Rechazada','Cancelada')),
                Motivo TEXT NULL,
                AprobadoPor TEXT NULL,
                FechaAprobacion TEXT NULL,
                ObservacionResolucion TEXT NULL,
                UsuarioSolicitante TEXT NULL,
                FechaCreacion TEXT NOT NULL,
                FOREIGN KEY(IdEmpleado) REFERENCES Empleados(IdEmpleado)
            );");
            AddColumnIfMissing(connection, transaction, "Asistencias", "IdSolicitudAusencia",
                "INTEGER NULL REFERENCES SolicitudesAusencia(IdSolicitud)");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_SolicitudesAusencia_EmpleadoFechas ON SolicitudesAusencia(IdEmpleado, FechaInicio, FechaFin);");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_SolicitudesAusencia_Estado ON SolicitudesAusencia(Estado, FechaInicio);");
            Execute(connection, transaction,
                "CREATE INDEX IF NOT EXISTS IX_Asistencias_IdSolicitudAusencia ON Asistencias(IdSolicitudAusencia);");

            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_Validar_Insert
                BEFORE INSERT ON SolicitudesAusencia
                WHEN NEW.FechaInicio > NEW.FechaFin OR NEW.Estado <> 'Pendiente'
                BEGIN SELECT RAISE(ABORT, 'La solicitud debe tener fechas validas e iniciar pendiente.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_Transicion_Update
                BEFORE UPDATE OF Estado ON SolicitudesAusencia
                WHEN OLD.Estado <> NEW.Estado AND NOT (
                    (OLD.Estado = 'Pendiente' AND NEW.Estado IN ('Aprobada','Rechazada','Cancelada')) OR
                    (OLD.Estado = 'Aprobada' AND NEW.Estado = 'Cancelada'))
                BEGIN SELECT RAISE(ABORT, 'La transicion de la solicitud de ausencia no es valida.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_PeriodoCerrado_Insert
                BEFORE INSERT ON SolicitudesAusencia
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina WHERE Cerrado = 1
                    AND FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)
                BEGIN SELECT RAISE(ABORT, 'La ausencia afecta un periodo de nomina cerrado.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_PeriodoCerrado_Update
                BEFORE UPDATE ON SolicitudesAusencia
                WHEN EXISTS (SELECT 1 FROM PeriodosNomina WHERE Cerrado = 1
                    AND ((FechaInicio <= OLD.FechaFin AND FechaFin >= OLD.FechaInicio)
                      OR (FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)))
                BEGIN SELECT RAISE(ABORT, 'La ausencia afecta un periodo de nomina cerrado.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_AprobadaSolapada_Insert
                BEFORE INSERT ON SolicitudesAusencia
                WHEN NEW.Estado = 'Aprobada' AND EXISTS (SELECT 1 FROM SolicitudesAusencia
                    WHERE IdEmpleado = NEW.IdEmpleado AND Estado = 'Aprobada'
                      AND FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)
                BEGIN SELECT RAISE(ABORT, 'Ya existe una ausencia aprobada que se solapa con este rango.'); END;");
            Execute(connection, transaction, @"CREATE TRIGGER IF NOT EXISTS TR_SolicitudesAusencia_AprobadaSolapada_Update
                BEFORE UPDATE OF Estado, FechaInicio, FechaFin, IdEmpleado ON SolicitudesAusencia
                WHEN NEW.Estado = 'Aprobada' AND EXISTS (SELECT 1 FROM SolicitudesAusencia
                    WHERE IdSolicitud <> OLD.IdSolicitud AND IdEmpleado = NEW.IdEmpleado AND Estado = 'Aprobada'
                      AND FechaInicio <= NEW.FechaFin AND FechaFin >= NEW.FechaInicio)
                BEGIN SELECT RAISE(ABORT, 'Ya existe una ausencia aprobada que se solapa con este rango.'); END;");

            detail.Append("Solicitudes de ausencia, alcance de asistencia, indices y reglas de integridad instalados.");
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
