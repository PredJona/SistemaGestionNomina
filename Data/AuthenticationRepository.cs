using System;
using System.Data.SQLite;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Data
{
    public sealed class AuthenticationRepository
    {
        public Usuario GetByUsername(string username)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(
                "SELECT * FROM Usuarios WHERE NombreUsuario = @username LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        public void RegisterFailedAttempt(int userId, int attempts, bool blocked, DateTime? blockedAt)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Usuarios SET
                IntentosFallidos = @attempts, Bloqueado = @blocked, FechaBloqueo = @blockedAt
                WHERE IdUsuario = @id;", connection))
            {
                command.Parameters.AddWithValue("@attempts", attempts);
                command.Parameters.AddWithValue("@blocked", blocked ? 1 : 0);
                command.Parameters.AddWithValue("@blockedAt", blockedAt.HasValue
                    ? (object)blockedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value);
                command.Parameters.AddWithValue("@id", userId);
                command.ExecuteNonQuery();
            }
        }

        public void CompleteLogin(int userId, DateTime lastAccess, string upgradedHash)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Usuarios SET
                UltimoAcceso = @access, IntentosFallidos = 0, Bloqueado = 0, FechaBloqueo = NULL,
                PasswordHash = CASE WHEN @hash = '' THEN PasswordHash ELSE @hash END
                WHERE IdUsuario = @id;", connection, transaction))
            {
                command.Parameters.AddWithValue("@access", lastAccess.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@hash", upgradedHash ?? string.Empty);
                command.Parameters.AddWithValue("@id", userId);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        private static Usuario Map(SQLiteDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                NombreUsuario = Convert.ToString(reader["NombreUsuario"]),
                PasswordHash = Convert.ToString(reader["PasswordHash"]),
                Rol = Convert.ToString(reader["Rol"]),
                Estado = Convert.ToString(reader["Estado"]),
                IdEmpleado = reader["IdEmpleado"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdEmpleado"]),
                UltimoAcceso = ParseNullableDate(reader["UltimoAcceso"]),
                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                Bloqueado = Convert.ToInt32(reader["Bloqueado"]) == 1,
                FechaBloqueo = ParseNullableDate(reader["FechaBloqueo"])
            };
        }

        private static DateTime? ParseNullableDate(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(value))) return null;
            DateTime parsed;
            return DateTime.TryParse(Convert.ToString(value), out parsed) ? parsed : (DateTime?)null;
        }
    }
}
