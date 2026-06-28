using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Data;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio reservado para registrar historial detallado de cambios.
    /// </summary>
    public class AuditTrailService
    {
        /// <summary>
        /// Registra una acción del usuario en una bitácora de auditoría.
        /// </summary>
        public void RegistrarCambio(string usuario, string modulo, string accion)
        {
            RegistrarCambio(usuario, modulo, accion, string.Empty);
        }

        /// <summary>
        /// Registra una acción con detalle adicional para seguimiento operativo.
        /// </summary>
        public void RegistrarCambio(string usuario, string modulo, string accion, string detalle)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                EnsureTable(connection);
                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Auditoria
                    (Usuario, Modulo, Accion, Detalle, Fecha)
                    VALUES (@usuario, @modulo, @accion, @detalle, @fecha);", connection))
                {
                    command.Parameters.AddWithValue("@usuario", Normalize(usuario, "sistema"));
                    command.Parameters.AddWithValue("@modulo", Normalize(modulo, "General"));
                    command.Parameters.AddWithValue("@accion", Normalize(accion, "Accion"));
                    command.Parameters.AddWithValue("@detalle", Normalize(detalle, string.Empty));
                    command.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Obtiene las últimas acciones registradas para diagnóstico o reportes.
        /// </summary>
        public List<string> ObtenerUltimos(int cantidad)
        {
            List<string> items = new List<string>();
            int limit = cantidad <= 0 ? 20 : Math.Min(cantidad, 200);

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                EnsureTable(connection);
                using (SQLiteCommand command = new SQLiteCommand(@"SELECT Usuario, Modulo, Accion, Detalle, Fecha
                    FROM Auditoria ORDER BY Fecha DESC, IdAuditoria DESC LIMIT @limit;", connection))
                {
                    command.Parameters.AddWithValue("@limit", limit);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(Convert.ToString(reader["Fecha"]) + " | " +
                                Convert.ToString(reader["Usuario"]) + " | " +
                                Convert.ToString(reader["Modulo"]) + " | " +
                                Convert.ToString(reader["Accion"]) + " | " +
                                Convert.ToString(reader["Detalle"]));
                        }
                    }
                }
            }

            return items;
        }

        private static void EnsureTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Auditoria (
                IdAuditoria INTEGER PRIMARY KEY AUTOINCREMENT,
                Usuario TEXT NOT NULL,
                Modulo TEXT NOT NULL,
                Accion TEXT NOT NULL,
                Detalle TEXT,
                Fecha TEXT NOT NULL
            );", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string Normalize(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }
    }
}
