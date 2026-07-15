using System;
using System.Data.SQLite;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

internal static class Fase3NominaSmokeHost
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Length != 1) throw new ArgumentException("Debe indicar la base temporal.");
            Environment.SetEnvironmentVariable("NOMINA_DB_PATH", args[0]);
            DatabaseInitializer.Initialize();
            DatabaseInitializer.Initialize();
            Assert(ScalarInt("PRAGMA user_version;") == DatabaseMigrationRunner.LatestVersion,
                "Las migraciones no alcanzaron la última versión.");
            Assert(ScalarInt("SELECT COUNT(1) FROM MigracionesLog WHERE Version IN (4,5,6);") == 3,
                "No se registraron las migraciones 4, 5 y 6.");

            SessionContext.Begin(new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "admin_smoke_f3",
                Rol = Roles.Admin,
                Estado = "Activo"
            });

            DateTime januaryStart = new DateTime(2034, 1, 1);
            DateTime januaryEnd = new DateTime(2034, 1, 15);
            AsistenciaService attendanceService = new AsistenciaService();
            attendanceService.Register(new Asistencia
            {
                IdEmpleado = 1,
                Fecha = new DateTime(2034, 1, 2),
                HoraEntrada = new TimeSpan(8, 0, 0),
                HoraSalida = new TimeSpan(17, 0, 0),
                Estado = "Puntual"
            });

            NominaService payrollService = new NominaService();
            Nomina january = payrollService.CalcularNomina(januaryStart, januaryEnd, null);
            int januaryId = payrollService.ConfirmarPago(january, januaryStart, januaryEnd);
            Assert(ScalarInt("SELECT COUNT(1) FROM PeriodosNomina WHERE Cerrado=1 AND FechaInicio='2034-01-01' AND FechaFin='2034-01-15';") == 1,
                "La confirmación no cerró el período.");

            ExpectFailure(delegate
            {
                attendanceService.Register(new Asistencia
                {
                    IdEmpleado = 1, Fecha = new DateTime(2034, 1, 3),
                    HoraEntrada = new TimeSpan(8, 0, 0), HoraSalida = new TimeSpan(17, 0, 0), Estado = "Puntual"
                });
            }, "El servicio permitió asistencia en período cerrado.");
            ExpectSqlFailure("INSERT INTO Asistencias (IdEmpleado,Fecha,HorasTrabajadas,Estado) VALUES (1,'2034-01-04',0,'Falta');",
                "El trigger permitió insertar asistencia en período cerrado.");
            ExpectSqlFailure("UPDATE Asistencias SET Estado='Tardanza' WHERE IdEmpleado=1 AND Fecha='2034-01-02';",
                "El trigger permitió actualizar asistencia en período cerrado.");
            ExpectFailure(delegate { payrollService.CalcularNomina(new DateTime(2034, 1, 5), new DateTime(2034, 1, 20), null); },
                "Se permitió calcular un rango solapado.");
            ExpectSqlFailure(@"INSERT INTO PeriodosNomina
                (Nombre,FechaInicio,FechaFin,Estado,Cerrado) VALUES ('Solapado','2034-01-10','2034-01-20','Confirmado',1);",
                "SQLite permitió un período protegido solapado.");

            payrollService.PagarNomina(januaryId);
            Assert(ScalarString("SELECT Estado FROM Nominas WHERE IdNomina=" + januaryId + ";") == PayrollStates.Paid,
                "La nómina confirmada no pasó a Pagada.");
            ExpectFailure(delegate { payrollService.PagarNomina(januaryId); }, "Se pagó dos veces la misma nómina.");
            payrollService.AnularNomina(januaryId, "Corrección integral de smoke test");
            Assert(ScalarString("SELECT Estado FROM Nominas WHERE IdNomina=" + januaryId + ";") == PayrollStates.Annulled,
                "La nómina pagada no se anuló con trazabilidad.");
            Assert(ScalarInt("SELECT COUNT(1) FROM NominaDetalle WHERE IdNomina=" + januaryId + ";") > 0 &&
                ScalarInt("SELECT COUNT(1) FROM Comprobantes WHERE IdNomina=" + januaryId + ";") > 0,
                "La anulación eliminó detalles o comprobantes.");
            ExpectFailure(delegate { payrollService.PagarNomina(januaryId); }, "Se permitió pagar una nómina anulada.");

            Nomina januaryRecalculated = payrollService.CalcularNomina(januaryStart, januaryEnd, null);
            int januaryNewId = payrollService.RecalcularNomina(januaryId, januaryStart, januaryEnd, januaryRecalculated);
            Assert(ScalarInt("SELECT COUNT(1) FROM NominaVersiones WHERE IdNominaOriginal=" + januaryId +
                " AND IdNominaNueva=" + januaryNewId + ";") == 1, "El recálculo no vinculó la versión nueva.");

            DateTime februaryStart = new DateTime(2034, 2, 1);
            DateTime februaryEnd = new DateTime(2034, 2, 15);
            Nomina february = payrollService.CalcularNomina(februaryStart, februaryEnd, null);
            int februaryId = payrollService.ConfirmarPago(february, februaryStart, februaryEnd);
            payrollService.AnularNomina(februaryId, "Preparar prueba de rollback");
            Nomina februaryRecalculated = payrollService.CalcularNomina(februaryStart, februaryEnd, null);
            Execute(@"CREATE TRIGGER Smoke_Fallar_Version BEFORE UPDATE ON NominaVersiones
                BEGIN SELECT RAISE(ABORT, 'Fallo controlado de version'); END;");
            int periodCount = ScalarInt("SELECT COUNT(1) FROM PeriodosNomina;");
            int payrollCount = ScalarInt("SELECT COUNT(1) FROM Nominas;");
            int detailCount = ScalarInt("SELECT COUNT(1) FROM NominaDetalle;");
            int payslipCount = ScalarInt("SELECT COUNT(1) FROM Comprobantes;");
            int auditCount = ScalarInt("SELECT COUNT(1) FROM Auditoria;");
            ExpectFailure(delegate
            {
                payrollService.RecalcularNomina(februaryId, februaryStart, februaryEnd, februaryRecalculated);
            }, "El recálculo no propagó el fallo controlado.");
            Execute("DROP TRIGGER Smoke_Fallar_Version;");
            Assert(periodCount == ScalarInt("SELECT COUNT(1) FROM PeriodosNomina;") &&
                payrollCount == ScalarInt("SELECT COUNT(1) FROM Nominas;") &&
                detailCount == ScalarInt("SELECT COUNT(1) FROM NominaDetalle;") &&
                payslipCount == ScalarInt("SELECT COUNT(1) FROM Comprobantes;") &&
                auditCount == ScalarInt("SELECT COUNT(1) FROM Auditoria;"),
                "El recálculo fallido dejó datos parciales.");

            EmpleadoService employeeService = new EmpleadoService();
            Empleado blocked = employeeService.GetById(1);
            blocked.SalarioBase += 10m;
            ExpectFailure(delegate
            {
                employeeService.Save(blocked, new EmployeeChangeContext
                {
                    EffectiveDate = new DateTime(2034, 1, 10), Reason = "Cambio que afectaría período cerrado"
                });
            }, "Se permitió un cambio laboral que afecta nómina cerrada.");

            Empleado scheduled = employeeService.GetById(1);
            decimal originalSalary = scheduled.SalarioBase;
            string originalDepartment = scheduled.DepartamentoNombre;
            scheduled.SalarioBase += 175m;
            scheduled.Cargo = scheduled.Cargo + " II";
            scheduled.IdDepartamento = 2;
            scheduled.Estado = "Inactivo";
            EmployeeSaveResult saveResult = employeeService.Save(scheduled, new EmployeeChangeContext
            {
                EffectiveDate = new DateTime(2034, 1, 20), Reason = "Prueba de cambios laborales programados"
            });
            Assert(saveResult.HistoryRecords == 4 && saveResult.HasScheduledChanges,
                "No se registraron los cuatro cambios programados.");
            Assert(employeeService.GetById(1).SalarioBase == originalSalary,
                "Un cambio futuro se aplicó antes de su fecha.");
            new EmployeeHistoryService().ApplyDueChanges(new DateTime(2034, 1, 20));
            Assert(employeeService.GetById(1).SalarioBase == originalSalary + 175m &&
                employeeService.GetById(1).Estado == "Inactivo", "Los cambios vencidos no se aplicaron.");
            Assert(ScalarInt("SELECT COUNT(1) FROM HistorialEmpleado WHERE IdEmpleado=1;") == 4,
                "El historial laboral no conservó todos los campos.");
            Assert(ScalarDecimal("SELECT SueldoBase FROM NominaDetalle WHERE IdNomina=" + januaryNewId + " AND IdEmpleado=1;") == originalSalary &&
                ScalarString("SELECT DepartamentoSnapshot FROM NominaDetalle WHERE IdNomina=" + januaryNewId + " AND IdEmpleado=1;") == originalDepartment,
                "Los snapshots históricos cambiaron después de modificar el empleado.");
            Assert(ScalarInt("SELECT COUNT(1) FROM Auditoria WHERE Usuario='admin_smoke_f3';") > 0,
                "La auditoría no registró al usuario real.");

            SessionContext.Clear();
            SQLiteConnection.ClearAllPools();
            Console.WriteLine("Migraciones, periodos, estados, rollback, historial y snapshots OK");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            try { SessionContext.Clear(); SQLiteConnection.ClearAllPools(); } catch { }
            return 1;
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void ExpectFailure(Action action, string message)
    {
        try { action(); }
        catch { return; }
        throw new InvalidOperationException(message);
    }

    private static void ExpectSqlFailure(string sql, string message)
    {
        try { Execute(sql); }
        catch (SQLiteException) { return; }
        throw new InvalidOperationException(message);
    }

    private static void Execute(string sql)
    {
        using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) command.ExecuteNonQuery();
    }

    private static object Scalar(string sql)
    {
        using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) return command.ExecuteScalar();
    }

    private static int ScalarInt(string sql) { return Convert.ToInt32(Scalar(sql)); }
    private static decimal ScalarDecimal(string sql) { return Convert.ToDecimal(Scalar(sql)); }
    private static string ScalarString(string sql) { return Convert.ToString(Scalar(sql)); }
}
