using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Data
{
    public sealed class AuditRepository
    {
        public void Add(AuditRecord record)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Auditoria
                (Usuario, Modulo, Accion, Detalle, Fecha)
                VALUES (@usuario, @modulo, @accion, @detalle, @fecha);", connection))
            {
                command.Parameters.AddWithValue("@usuario", record.Usuario);
                command.Parameters.AddWithValue("@modulo", record.Modulo);
                command.Parameters.AddWithValue("@accion", record.Accion);
                command.Parameters.AddWithValue("@detalle", record.Detalle ?? string.Empty);
                command.Parameters.AddWithValue("@fecha", record.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        public List<AuditRecord> Search(AuditQuery query)
        {
            List<AuditRecord> records = new List<AuditRecord>();
            int limit = query == null || query.Limit <= 0 ? 100 : Math.Min(query.Limit, 500);
            int offset = query == null ? 0 : Math.Max(0, query.Offset);
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT * FROM Auditoria
                WHERE (@usuario = '' OR Usuario LIKE @usuarioLike)
                  AND (@modulo = '' OR Modulo = @modulo)
                  AND (@accion = '' OR Accion = @accion)
                  AND (@detalle = '' OR Detalle LIKE @detalleLike)
                  AND (@desde = '' OR Fecha >= @desde)
                  AND (@hasta = '' OR Fecha < @hasta)
                ORDER BY Fecha DESC, IdAuditoria DESC LIMIT @limit OFFSET @offset;", connection))
            {
                string user = query == null ? string.Empty : query.Usuario ?? string.Empty;
                string module = query == null ? string.Empty : query.Modulo ?? string.Empty;
                string action = query == null ? string.Empty : query.Accion ?? string.Empty;
                string detail = query == null ? string.Empty : query.Detalle ?? string.Empty;
                command.Parameters.AddWithValue("@usuario", user);
                command.Parameters.AddWithValue("@usuarioLike", "%" + user + "%");
                command.Parameters.AddWithValue("@modulo", module);
                command.Parameters.AddWithValue("@accion", action);
                command.Parameters.AddWithValue("@detalle", detail);
                command.Parameters.AddWithValue("@detalleLike", "%" + detail + "%");
                command.Parameters.AddWithValue("@desde", query != null && query.FechaDesde.HasValue
                    ? query.FechaDesde.Value.Date.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                command.Parameters.AddWithValue("@hasta", query != null && query.FechaHasta.HasValue
                    ? query.FechaHasta.Value.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                command.Parameters.AddWithValue("@limit", limit);
                command.Parameters.AddWithValue("@offset", offset);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) records.Add(Map(reader));
                }
            }

            return records;
        }

        public List<string> GetDistinct(string column)
        {
            if (column != "Modulo" && column != "Accion") throw new ArgumentException("Columna de auditoría no válida.");
            List<string> values = new List<string>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(
                "SELECT DISTINCT " + column + " FROM Auditoria ORDER BY " + column + ";", connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read()) values.Add(Convert.ToString(reader[0]));
            }
            return values;
        }

        private static AuditRecord Map(SQLiteDataReader reader)
        {
            return new AuditRecord
            {
                IdAuditoria = Convert.ToInt32(reader["IdAuditoria"]),
                Usuario = Convert.ToString(reader["Usuario"]),
                Modulo = Convert.ToString(reader["Modulo"]),
                Accion = Convert.ToString(reader["Accion"]),
                Detalle = Convert.ToString(reader["Detalle"]),
                Fecha = DateTime.Parse(Convert.ToString(reader["Fecha"]))
            };
        }
    }
}
