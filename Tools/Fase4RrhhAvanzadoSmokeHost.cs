using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

internal static class Fase4RrhhAvanzadoSmokeHost
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Length != 1) throw new ArgumentException("Debe indicar la base temporal.");
            Environment.SetEnvironmentVariable("NOMINA_DB_PATH", args[0]);
            DatabaseInitializer.Initialize();
            Assert(ScalarInt("PRAGMA user_version;") == 6, "La base temporal no llegó a la migración v6.");
            Assert(ScalarInt("SELECT COUNT(1) FROM pragma_table_info('Asistencias') WHERE name='IdSolicitudAusencia';") == 1,
                "Falta la relación entre asistencia y solicitud.");

            RolePermissionService roles = new RolePermissionService();
            Assert(roles.TienePermiso(Roles.RRHH, Permissions.AbsencesApprove), "RRHH no recibió gestión de ausencias.");
            Assert(roles.TienePermiso(Roles.Supervisor, Permissions.AbsencesApprove), "Supervisor no recibió aprobación departamental.");
            Assert(!roles.TienePermiso(Roles.Contabilidad, Permissions.AbsencesApprove) &&
                roles.TienePermiso(Roles.Contabilidad, Permissions.AbsencesView), "Contabilidad tiene una matriz incorrecta.");
            Assert(roles.TienePermiso(Roles.Trabajador, Permissions.OwnAbsencesCreate) &&
                !roles.TienePermiso(Roles.Trabajador, Permissions.AbsencesView), "Trabajador recibió alcance administrativo.");

            DateTime monday = NextMonday(new DateTime(2035, 3, 1));
            AbsenceRequestService service = new AbsenceRequestService();
            BeginSession(40, "trabajador_f4", Roles.Trabajador, 1);
            int ownId = service.CreateOwn(new OwnAbsenceRequest
            {
                Type = AbsenceTypes.PaidLeave,
                StartDate = monday,
                EndDate = monday.AddDays(1),
                Reason = "Gestión personal de prueba"
            });
            List<SolicitudAusencia> ownItems = service.Search(new AbsenceQuery());
            Assert(ownItems.Count == 1 && ownItems[0].IdEmpleado == 1, "El portal no aisló solicitudes propias.");

            BeginSession(1, "admin_f4", Roles.Admin, null);
            int employeeTwoId = service.CreateForEmployee(NewRequest(2, AbsenceTypes.Vacation,
                monday, monday.AddDays(4), "Vacaciones de prueba"));
            BeginSession(40, "trabajador_f4", Roles.Trabajador, 1);
            ExpectUnauthorized(delegate { service.GetById(employeeTwoId); }, "El trabajador consultó una solicitud ajena.");
            service.Cancel(ownId, "Cancelación personal de prueba");
            Assert(ScalarString("SELECT Estado FROM SolicitudesAusencia WHERE IdSolicitud=" + ownId + ";") == AbsenceStates.Cancelled,
                "El trabajador no pudo cancelar su solicitud pendiente.");

            BeginSession(1, "admin_f4", Roles.Admin, null);
            service.Approve(employeeTwoId, "Aprobada por RRHH de prueba");
            Assert(ScalarInt("SELECT COUNT(1) FROM Asistencias WHERE IdSolicitudAusencia=" + employeeTwoId + ";") == 5,
                "La aprobación no creó exactamente cinco días laborables.");
            Assert(ScalarInt("SELECT COUNT(1) FROM Asistencias WHERE IdSolicitudAusencia=" + employeeTwoId + " AND Estado='Permiso';") == 5,
                "Vacaciones no se reflejaron como Permiso.");

            int overlapId = service.CreateForEmployee(NewRequest(2, AbsenceTypes.MedicalLeave,
                monday.AddDays(2), monday.AddDays(4), "Solicitud solapada"));
            ExpectFailure(delegate { service.Approve(overlapId, "No debe aprobar"); },
                "Se aprobó una ausencia solapada.");
            Assert(ScalarString("SELECT Estado FROM SolicitudesAusencia WHERE IdSolicitud=" + overlapId + ";") == AbsenceStates.Pending,
                "La aprobación fallida cambió el estado de la solicitud.");

            service.Cancel(employeeTwoId, "Cancelar ausencia aprobada");
            Assert(ScalarInt("SELECT COUNT(1) FROM Asistencias WHERE IdSolicitudAusencia=" + employeeTwoId + ";") == 0,
                "La cancelación no retiró las asistencias generadas.");

            DateTime conflictDate = monday.AddDays(14);
            Execute("INSERT INTO Asistencias (IdEmpleado,Fecha,HoraEntrada,HoraSalida,HorasTrabajadas,Estado) VALUES (2,'" +
                conflictDate.ToString("yyyy-MM-dd") + "','08:00','17:00',9,'Puntual');");
            int conflictId = service.CreateForEmployee(NewRequest(2, AbsenceTypes.UnpaidLeave,
                conflictDate, conflictDate, "Permiso con asistencia existente"));
            ExpectFailure(delegate { service.Approve(conflictId, "Debe fallar sin sobrescribir"); },
                "Se sobrescribió una asistencia existente.");
            Assert(ScalarInt("SELECT COUNT(1) FROM Asistencias WHERE IdEmpleado=2 AND Fecha='" +
                conflictDate.ToString("yyyy-MM-dd") + "';") == 1 &&
                ScalarString("SELECT Estado FROM SolicitudesAusencia WHERE IdSolicitud=" + conflictId + ";") == AbsenceStates.Pending,
                "El conflicto de asistencia dejó una operación parcial.");

            DateTime suspensionDate = monday.AddDays(21);
            int suspensionId = service.CreateForEmployee(NewRequest(3, AbsenceTypes.Suspension,
                suspensionDate, suspensionDate, "Suspensión académica"));
            service.Approve(suspensionId, "Aprobada");
            Assert(ScalarString("SELECT Estado FROM Asistencias WHERE IdSolicitudAusencia=" + suspensionId + ";") == "Falta",
                "La suspensión no se reflejó como Falta.");

            DateTime supervisorDate = monday.AddDays(28);
            int departmentRequestId = service.CreateForEmployee(NewRequest(4, AbsenceTypes.License,
                supervisorDate, supervisorDate.AddDays(1), "Solicitud de Operaciones"));
            int foreignDepartmentRequestId = service.CreateForEmployee(NewRequest(2, AbsenceTypes.PaidLeave,
                supervisorDate, supervisorDate, "Solicitud de Ventas"));
            BeginSession(50, "supervisor_f4", Roles.Supervisor, 4);
            service.Approve(departmentRequestId, "Aprobación departamental");
            ExpectUnauthorized(delegate { service.Reject(foreignDepartmentRequestId, "Fuera de alcance"); },
                "El supervisor resolvió una solicitud de otro departamento.");
            List<SolicitudAusencia> scoped = service.Search(new AbsenceQuery());
            for (int i = 0; i < scoped.Count; i++)
                Assert(scoped[i].DepartamentoNombre == "Operaciones", "La consulta del supervisor filtró otro departamento.");

            BeginSession(60, "contabilidad_f4", Roles.Contabilidad, null);
            Assert(service.Search(new AbsenceQuery()).Count > 0, "Contabilidad no pudo consultar ausencias.");
            ExpectUnauthorized(delegate { service.Approve(foreignDepartmentRequestId, "No autorizado"); },
                "Contabilidad aprobó una solicitud.");

            BeginSession(1, "admin_f4", Roles.Admin, null);
            DateTime pendingClosedDate = monday.AddDays(70);
            int pendingClosedId = service.CreateForEmployee(NewRequest(3, AbsenceTypes.JustifiedAbsence,
                pendingClosedDate, pendingClosedDate, "Pendiente antes del cierre"));
            InsertClosedPeriod(pendingClosedDate, pendingClosedDate, "Cierre posterior");
            ExpectFailure(delegate { service.Reject(pendingClosedId, "No puede alterar período cerrado"); },
                "Se modificó una ausencia después de cerrar el período.");
            ExpectSqlFailure("UPDATE SolicitudesAusencia SET Estado='Rechazada' WHERE IdSolicitud=" + pendingClosedId + ";",
                "SQLite permitió modificar una ausencia de período cerrado.");

            DateTime closedStart = monday.AddDays(84);
            InsertClosedPeriod(closedStart, closedStart.AddDays(4), "Período cerrado de ausencia");
            ExpectFailure(delegate
            {
                service.CreateForEmployee(NewRequest(2, AbsenceTypes.Vacation,
                    closedStart, closedStart.AddDays(1), "Debe bloquearse"));
            }, "Se creó una solicitud dentro de un período cerrado.");
            ExpectSqlFailure(@"INSERT INTO SolicitudesAusencia
                (IdEmpleado,Tipo,FechaInicio,FechaFin,Estado,Motivo,UsuarioSolicitante,FechaCreacion)
                VALUES (2,'Vacaciones','" + closedStart.ToString("yyyy-MM-dd") + "','" +
                closedStart.AddDays(1).ToString("yyyy-MM-dd") + "','Pendiente','Directa','sql','2035-01-01');",
                "SQLite permitió insertar una ausencia dentro de un período cerrado.");

            Assert(ScalarInt("SELECT COUNT(1) FROM Auditoria WHERE Modulo='Ausencias' AND Usuario IN ('admin_f4','trabajador_f4','supervisor_f4');") > 0,
                "La auditoría no registró a los usuarios reales de ausencias.");
            SessionContext.Clear();
            SQLiteConnection.ClearAllPools();
            Console.WriteLine("Permisos, alcance, ausencias, asistencia, cancelación y periodos cerrados OK");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            try { SessionContext.Clear(); SQLiteConnection.ClearAllPools(); } catch { }
            return 1;
        }
    }

    private static SolicitudAusencia NewRequest(int employeeId, string type, DateTime start, DateTime end, string reason)
    {
        return new SolicitudAusencia { IdEmpleado = employeeId, Tipo = type, FechaInicio = start, FechaFin = end, Motivo = reason };
    }

    private static void BeginSession(int id, string username, string role, int? employeeId)
    {
        SessionContext.Begin(new Usuario
        {
            IdUsuario = id, NombreUsuario = username, Rol = role, Estado = "Activo", IdEmpleado = employeeId
        });
    }

    private static DateTime NextMonday(DateTime date)
    {
        DateTime result = date.Date;
        while (result.DayOfWeek != DayOfWeek.Monday) result = result.AddDays(1);
        return result;
    }

    private static void InsertClosedPeriod(DateTime start, DateTime end, string name)
    {
        using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
        using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO PeriodosNomina
            (Nombre,FechaInicio,FechaFin,Estado,Cerrado,FechaCierre,CerradoPor)
            VALUES (@nombre,@inicio,@fin,'Confirmado',1,@cierre,'admin_f4');", connection))
        {
            command.Parameters.AddWithValue("@nombre", name);
            command.Parameters.AddWithValue("@inicio", start.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@fin", end.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@cierre", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            command.ExecuteNonQuery();
        }
    }

    private static void Assert(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void ExpectFailure(Action action, string message) { try { action(); } catch { return; } throw new InvalidOperationException(message); }
    private static void ExpectUnauthorized(Action action, string message)
    {
        try { action(); }
        catch (UnauthorizedAccessException) { return; }
        throw new InvalidOperationException(message);
    }
    private static void ExpectSqlFailure(string sql, string message)
    {
        try { Execute(sql); } catch (SQLiteException) { return; }
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
    private static string ScalarString(string sql) { return Convert.ToString(Scalar(sql)); }
}
