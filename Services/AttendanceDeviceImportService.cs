using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio destinado a importar asistencia desde reloj marcador o archivos externos.
    /// </summary>
    public class AttendanceDeviceImportService
    {
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();

        /// <summary>
        /// Importa registros externos de asistencia desde CSV: Codigo,Fecha,HoraEntrada,HoraSalida,Estado.
        /// </summary>
        public int ImportarDesdeArchivo(string rutaArchivo)
        {
            authorizationService.DemandPermission(Permissions.AttendanceImport);
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("Debe indicar el archivo de asistencia.");
            }

            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("No se encontró el archivo de asistencia.", rutaArchivo);
            }

            string[] lines = File.ReadAllLines(rutaArchivo);
            if (lines.Length == 0)
            {
                return 0;
            }

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                Dictionary<string, int> empleados = CargarEmpleados(connection, transaction);
                int imported = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                    {
                        continue;
                    }

                    string[] values = SplitCsvLine(lines[i]);
                    if (i == 0 && values.Length > 0 && values[0].Trim().Equals("Codigo", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (values.Length < 5)
                    {
                        throw new InvalidDataException("Formato inválido en línea " + (i + 1) + ". Use Codigo,Fecha,HoraEntrada,HoraSalida,Estado.");
                    }

                    string codigo = values[0].Trim();
                    if (!empleados.ContainsKey(codigo))
                    {
                        throw new InvalidDataException("Empleado no encontrado en línea " + (i + 1) + ": " + codigo);
                    }

                    DateTime fecha = ParseDate(values[1], i + 1);
                    TimeSpan? entrada = ParseTime(values[2], i + 1, false);
                    TimeSpan? salida = ParseTime(values[3], i + 1, false);
                    string estado = NormalizeStatus(values[4], i + 1);
                    decimal horas = CalculateHours(estado, entrada, salida, i + 1);

                    UpsertAsistencia(connection, transaction, empleados[codigo], fecha, entrada, salida, horas, estado);
                    imported++;
                }

                transaction.Commit();
                auditTrailService.RegistrarAccion("Asistencia", "Importar",
                    Path.GetFileName(rutaArchivo) + ", Registros=" + imported);
                return imported;
            }
        }

        private static Dictionary<string, int> CargarEmpleados(SQLiteConnection connection, SQLiteTransaction transaction)
        {
            Dictionary<string, int> empleados = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            using (SQLiteCommand command = new SQLiteCommand("SELECT IdEmpleado, Codigo FROM Empleados WHERE Estado = 'Activo';", connection, transaction))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    empleados[Convert.ToString(reader["Codigo"])] = Convert.ToInt32(reader["IdEmpleado"]);
                }
            }

            return empleados;
        }

        private static string[] SplitCsvLine(string line)
        {
            List<string> values = new List<string>();
            bool insideQuotes = false;
            string current = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (c == ',' && !insideQuotes)
                {
                    values.Add(current);
                    current = string.Empty;
                }
                else
                {
                    current += c;
                }
            }

            values.Add(current);
            return values.ToArray();
        }

        private static DateTime ParseDate(string value, int lineNumber)
        {
            DateTime date;
            string text = (value ?? string.Empty).Trim();
            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy" };
            if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ||
                DateTime.TryParse(text, out date))
            {
                return date.Date;
            }

            throw new InvalidDataException("Fecha inválida en línea " + lineNumber + ".");
        }

        private static TimeSpan? ParseTime(string value, int lineNumber, bool required)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0)
            {
                if (required)
                {
                    throw new InvalidDataException("Hora obligatoria en línea " + lineNumber + ".");
                }

                return null;
            }

            TimeSpan time;
            if (TimeSpan.TryParse(text, out time))
            {
                return time;
            }

            DateTime dateTime;
            if (DateTime.TryParse(text, out dateTime))
            {
                return dateTime.TimeOfDay;
            }

            throw new InvalidDataException("Hora inválida en línea " + lineNumber + ".");
        }

        private static string NormalizeStatus(string value, int lineNumber)
        {
            string status = (value ?? string.Empty).Trim();
            string[] allowed = { "Puntual", "Tardanza", "Falta", "Permiso" };
            for (int i = 0; i < allowed.Length; i++)
            {
                if (string.Equals(status, allowed[i], StringComparison.OrdinalIgnoreCase))
                {
                    return allowed[i];
                }
            }

            throw new InvalidDataException("Estado inválido en línea " + lineNumber + ". Use Puntual, Tardanza, Falta o Permiso.");
        }

        private static decimal CalculateHours(string estado, TimeSpan? entrada, TimeSpan? salida, int lineNumber)
        {
            if (estado == "Falta" || estado == "Permiso")
            {
                return 0m;
            }

            if (!entrada.HasValue || !salida.HasValue)
            {
                throw new InvalidDataException("Debe indicar entrada y salida en línea " + lineNumber + ".");
            }

            if (salida.Value <= entrada.Value)
            {
                throw new InvalidDataException("La salida debe ser mayor que la entrada en línea " + lineNumber + ".");
            }

            return Math.Round(Convert.ToDecimal((salida.Value - entrada.Value).TotalHours), 2);
        }

        private static void UpsertAsistencia(SQLiteConnection connection, SQLiteTransaction transaction, int idEmpleado, DateTime fecha, TimeSpan? entrada, TimeSpan? salida, decimal horas, string estado)
        {
            object existingId;
            using (SQLiteCommand find = new SQLiteCommand(
                "SELECT IdAsistencia FROM Asistencias WHERE IdEmpleado = @empleado AND Fecha = @fecha LIMIT 1;", connection, transaction))
            {
                find.Parameters.AddWithValue("@empleado", idEmpleado);
                find.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));
                existingId = find.ExecuteScalar();
            }

            string entradaText = entrada.HasValue ? entrada.Value.ToString(@"hh\:mm") : null;
            string salidaText = salida.HasValue ? salida.Value.ToString(@"hh\:mm") : null;

            if (existingId == null || existingId == DBNull.Value)
            {
                using (SQLiteCommand insert = new SQLiteCommand(@"INSERT INTO Asistencias
                    (IdEmpleado, Fecha, HoraEntrada, HoraSalida, HorasTrabajadas, Estado)
                    VALUES (@empleado, @fecha, @entrada, @salida, @horas, @estado);", connection, transaction))
                {
                    FillParameters(insert, idEmpleado, fecha, entradaText, salidaText, horas, estado);
                    insert.ExecuteNonQuery();
                }
            }
            else
            {
                using (SQLiteCommand update = new SQLiteCommand(@"UPDATE Asistencias SET
                    HoraEntrada = @entrada, HoraSalida = @salida, HorasTrabajadas = @horas, Estado = @estado
                    WHERE IdAsistencia = @id;", connection, transaction))
                {
                    FillParameters(update, idEmpleado, fecha, entradaText, salidaText, horas, estado);
                    update.Parameters.AddWithValue("@id", Convert.ToInt32(existingId));
                    update.ExecuteNonQuery();
                }
            }
        }

        private static void FillParameters(SQLiteCommand command, int idEmpleado, DateTime fecha, string entrada, string salida, decimal horas, string estado)
        {
            command.Parameters.AddWithValue("@empleado", idEmpleado);
            command.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@entrada", (object)entrada ?? DBNull.Value);
            command.Parameters.AddWithValue("@salida", (object)salida ?? DBNull.Value);
            command.Parameters.AddWithValue("@horas", horas);
            command.Parameters.AddWithValue("@estado", estado);
        }
    }
}
