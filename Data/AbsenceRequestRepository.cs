using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Data
{
    public sealed class AbsenceRequestRepository
    {
        private const string SelectSql = @"SELECT s.*, e.Codigo AS CodigoEmpleado,
                e.Nombre || ' ' || e.Apellido AS EmpleadoNombre,
                d.Nombre AS DepartamentoNombre
            FROM SolicitudesAusencia s
            INNER JOIN Empleados e ON e.IdEmpleado = s.IdEmpleado
            INNER JOIN Departamentos d ON d.IdDepartamento = e.IdDepartamento ";

        public List<SolicitudAusencia> Search(AbsenceQuery query, int? forcedEmployeeId,
            int? scopeDepartmentId)
        {
            query = query ?? new AbsenceQuery();
            List<SolicitudAusencia> items = new List<SolicitudAusencia>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(SelectSql + @"
                WHERE (@forcedEmployee = 0 OR s.IdEmpleado = @forcedEmployee)
                  AND (@empleado = 0 OR s.IdEmpleado = @empleado)
                  AND (@departamento = 0 OR e.IdDepartamento = @departamento)
                  AND (@scopeDep = 0 OR e.IdDepartamento = @scopeDep)
                  AND (@tipo = '' OR s.Tipo = @tipo)
                  AND (@estado = '' OR s.Estado = @estado)
                  AND (@desde = '' OR s.FechaFin >= @desde)
                  AND (@hasta = '' OR s.FechaInicio <= @hasta)
                  AND (@search = '' OR e.Codigo LIKE @like OR e.Nombre LIKE @like
                       OR e.Apellido LIKE @like OR s.Motivo LIKE @like)
                ORDER BY s.FechaCreacion DESC, s.IdSolicitud DESC;", connection))
            {
                command.Parameters.AddWithValue("@forcedEmployee", forcedEmployeeId ?? 0);
                command.Parameters.AddWithValue("@empleado", query.EmployeeId ?? 0);
                command.Parameters.AddWithValue("@departamento", query.DepartmentId ?? 0);
                command.Parameters.AddWithValue("@scopeDep", scopeDepartmentId ?? 0);
                command.Parameters.AddWithValue("@tipo", query.Type ?? string.Empty);
                command.Parameters.AddWithValue("@estado", query.Status ?? string.Empty);
                command.Parameters.AddWithValue("@desde", query.From.HasValue ? query.From.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@hasta", query.To.HasValue ? query.To.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@search", query.Search ?? string.Empty);
                command.Parameters.AddWithValue("@like", "%" + (query.Search ?? string.Empty) + "%");
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) items.Add(Map(reader));
                }
            }
            return items;
        }

        public SolicitudAusencia GetById(int requestId)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                return GetById(connection, null, requestId);
            }
        }

        public SolicitudAusencia GetById(SQLiteConnection connection, SQLiteTransaction transaction,
            int requestId)
        {
            using (SQLiteCommand command = new SQLiteCommand(SelectSql +
                " WHERE s.IdSolicitud = @id LIMIT 1;", connection, transaction))
            {
                command.Parameters.AddWithValue("@id", requestId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        public int Add(SQLiteConnection connection, SQLiteTransaction transaction, SolicitudAusencia item)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO SolicitudesAusencia
                (IdEmpleado, Tipo, FechaInicio, FechaFin, Estado, Motivo,
                 UsuarioSolicitante, FechaCreacion)
                VALUES (@empleado, @tipo, @inicio, @fin, @estado, @motivo, @usuario, @creacion);
                SELECT last_insert_rowid();", connection, transaction))
            {
                command.Parameters.AddWithValue("@empleado", item.IdEmpleado);
                command.Parameters.AddWithValue("@tipo", item.Tipo);
                command.Parameters.AddWithValue("@inicio", item.FechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", item.FechaFin.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@estado", AbsenceStates.Pending);
                command.Parameters.AddWithValue("@motivo", item.Motivo ?? string.Empty);
                command.Parameters.AddWithValue("@usuario", item.UsuarioSolicitante ?? string.Empty);
                command.Parameters.AddWithValue("@creacion", item.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss"));
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateState(SQLiteConnection connection, SQLiteTransaction transaction, int requestId,
            string expectedState, string newState, string resolvedBy, DateTime resolvedAt, string observation)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE SolicitudesAusencia
                SET Estado = @nuevo,
                    AprobadoPor = CASE WHEN @resolver = '' THEN AprobadoPor ELSE @resolver END,
                    FechaAprobacion = CASE WHEN @resolver = '' THEN FechaAprobacion ELSE @fecha END,
                    ObservacionResolucion = @observacion
                WHERE IdSolicitud = @id AND Estado = @anterior;", connection, transaction))
            {
                command.Parameters.AddWithValue("@nuevo", newState);
                command.Parameters.AddWithValue("@resolver", resolvedBy ?? string.Empty);
                command.Parameters.AddWithValue("@fecha", resolvedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@observacion", observation ?? string.Empty);
                command.Parameters.AddWithValue("@id", requestId);
                command.Parameters.AddWithValue("@anterior", expectedState);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("La solicitud cambió de estado antes de completar la operación.");
            }
        }

        public bool HasApprovedOverlap(SQLiteConnection connection, SQLiteTransaction transaction,
            int employeeId, DateTime start, DateTime end, int ignoreRequestId)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT COUNT(1) FROM SolicitudesAusencia
                WHERE IdEmpleado = @empleado AND Estado = 'Aprobada' AND IdSolicitud <> @ignorar
                  AND FechaInicio <= @fin AND FechaFin >= @inicio;", connection, transaction))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@inicio", start.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", end.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ignorar", ignoreRequestId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool AttendanceExists(SQLiteConnection connection, SQLiteTransaction transaction,
            int employeeId, DateTime date)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT COUNT(1) FROM Asistencias
                WHERE IdEmpleado = @empleado AND Fecha = @fecha;", connection, transaction))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@fecha", date.ToString("yyyy-MM-dd"));
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void AddGeneratedAttendance(SQLiteConnection connection, SQLiteTransaction transaction,
            int requestId, int employeeId, DateTime date, string status)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Asistencias
                (IdEmpleado, Fecha, HoraEntrada, HoraSalida, HorasTrabajadas, Estado, IdSolicitudAusencia)
                VALUES (@empleado, @fecha, NULL, NULL, 0, @estado, @solicitud);", connection, transaction))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@fecha", date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@estado", status);
                command.Parameters.AddWithValue("@solicitud", requestId);
                command.ExecuteNonQuery();
            }
        }

        public int DeleteGeneratedAttendance(SQLiteConnection connection, SQLiteTransaction transaction,
            int requestId)
        {
            using (SQLiteCommand command = new SQLiteCommand(
                "DELETE FROM Asistencias WHERE IdSolicitudAusencia = @solicitud;", connection, transaction))
            {
                command.Parameters.AddWithValue("@solicitud", requestId);
                return command.ExecuteNonQuery();
            }
        }

        private static SolicitudAusencia Map(SQLiteDataReader reader)
        {
            return new SolicitudAusencia
            {
                IdSolicitud = Convert.ToInt32(reader["IdSolicitud"]),
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                CodigoEmpleado = Convert.ToString(reader["CodigoEmpleado"]),
                EmpleadoNombre = Convert.ToString(reader["EmpleadoNombre"]),
                DepartamentoNombre = Convert.ToString(reader["DepartamentoNombre"]),
                Tipo = Convert.ToString(reader["Tipo"]),
                FechaInicio = DateTime.Parse(Convert.ToString(reader["FechaInicio"])),
                FechaFin = DateTime.Parse(Convert.ToString(reader["FechaFin"])),
                Estado = Convert.ToString(reader["Estado"]),
                Motivo = Convert.ToString(reader["Motivo"]),
                AprobadoPor = Convert.ToString(reader["AprobadoPor"]),
                FechaAprobacion = reader["FechaAprobacion"] == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(reader["FechaAprobacion"]))
                    ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaAprobacion"])),
                ObservacionResolucion = Convert.ToString(reader["ObservacionResolucion"]),
                UsuarioSolicitante = Convert.ToString(reader["UsuarioSolicitante"]),
                FechaCreacion = DateTime.Parse(Convert.ToString(reader["FechaCreacion"]))
            };
        }
    }
}
