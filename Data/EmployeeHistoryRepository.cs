using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Data
{
    public sealed class EmployeeHistoryRepository
    {
        private const string SelectSql = @"SELECT h.*, e.Codigo AS CodigoEmpleado,
                e.Nombre || ' ' || e.Apellido AS EmpleadoNombre
            FROM HistorialEmpleado h
            INNER JOIN Empleados e ON e.IdEmpleado = h.IdEmpleado ";

        public int Add(SQLiteConnection connection, SQLiteTransaction transaction, HistorialEmpleado item)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO HistorialEmpleado
                (IdEmpleado, CampoModificado, ValorAnterior, ValorNuevo,
                 ValorAnteriorTecnico, ValorNuevoTecnico, FechaCambio, FechaEfectiva,
                 UsuarioResponsable, Motivo, Aplicado, FechaAplicacion)
                VALUES (@empleado, @campo, @anterior, @nuevo, @anteriorTecnico, @nuevoTecnico,
                 @cambio, @efectiva, @usuario, @motivo, @aplicado, @aplicacion);
                SELECT last_insert_rowid();", connection, transaction))
            {
                command.Parameters.AddWithValue("@empleado", item.IdEmpleado);
                command.Parameters.AddWithValue("@campo", item.CampoModificado);
                command.Parameters.AddWithValue("@anterior", item.ValorAnterior ?? string.Empty);
                command.Parameters.AddWithValue("@nuevo", item.ValorNuevo ?? string.Empty);
                command.Parameters.AddWithValue("@anteriorTecnico", item.ValorAnteriorTecnico ?? item.ValorAnterior ?? string.Empty);
                command.Parameters.AddWithValue("@nuevoTecnico", item.ValorNuevoTecnico ?? item.ValorNuevo ?? string.Empty);
                command.Parameters.AddWithValue("@cambio", item.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@efectiva", item.FechaEfectiva.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@usuario", item.UsuarioResponsable);
                command.Parameters.AddWithValue("@motivo", item.Motivo ?? string.Empty);
                command.Parameters.AddWithValue("@aplicado", item.Aplicado ? 1 : 0);
                command.Parameters.AddWithValue("@aplicacion", item.FechaAplicacion.HasValue
                    ? (object)item.FechaAplicacion.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<HistorialEmpleado> Search(EmployeeHistoryQuery query, int? scopeDepartmentId)
        {
            query = query ?? new EmployeeHistoryQuery();
            List<HistorialEmpleado> items = new List<HistorialEmpleado>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(SelectSql + @"
                WHERE (@empleado = 0 OR h.IdEmpleado = @empleado)
                  AND (@scopeDep = 0 OR e.IdDepartamento = @scopeDep)
                  AND (@campo = '' OR h.CampoModificado = @campo)
                  AND (@usuario = '' OR h.UsuarioResponsable LIKE @usuarioLike)
                  AND (@desde = '' OR h.FechaEfectiva >= @desde)
                  AND (@hasta = '' OR h.FechaEfectiva <= @hasta)
                ORDER BY h.FechaEfectiva DESC, h.FechaCambio DESC, h.IdHistorial DESC;", connection))
            {
                command.Parameters.AddWithValue("@empleado", query.EmployeeId ?? 0);
                command.Parameters.AddWithValue("@scopeDep", scopeDepartmentId ?? 0);
                command.Parameters.AddWithValue("@campo", query.Field ?? string.Empty);
                command.Parameters.AddWithValue("@usuario", query.User ?? string.Empty);
                command.Parameters.AddWithValue("@usuarioLike", "%" + (query.User ?? string.Empty) + "%");
                command.Parameters.AddWithValue("@desde", query.From.HasValue ? query.From.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@hasta", query.To.HasValue ? query.To.Value.ToString("yyyy-MM-dd") : string.Empty);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) items.Add(Map(reader));
                }
            }
            return items;
        }

        public List<HistorialEmpleado> GetByEmployee(int employeeId)
        {
            List<HistorialEmpleado> items = new List<HistorialEmpleado>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(SelectSql + @"
                WHERE h.IdEmpleado = @empleado
                ORDER BY h.FechaEfectiva ASC, h.FechaCambio ASC, h.IdHistorial ASC;", connection))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) items.Add(Map(reader));
                }
            }
            return items;
        }

        public List<HistorialEmpleado> GetPendingDue(SQLiteConnection connection,
            SQLiteTransaction transaction, DateTime effectiveThrough)
        {
            List<HistorialEmpleado> items = new List<HistorialEmpleado>();
            using (SQLiteCommand command = new SQLiteCommand(SelectSql + @"
                WHERE h.Aplicado = 0 AND h.FechaEfectiva <= @fecha
                ORDER BY h.FechaEfectiva, h.FechaCambio, h.IdHistorial;", connection, transaction))
            {
                command.Parameters.AddWithValue("@fecha", effectiveThrough.ToString("yyyy-MM-dd"));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) items.Add(Map(reader));
                }
            }
            return items;
        }

        public void ApplyChange(SQLiteConnection connection, SQLiteTransaction transaction,
            HistorialEmpleado item, DateTime appliedAt)
        {
            string column;
            if (item.CampoModificado == EmployeeHistoryFields.BaseSalary) column = "SalarioBase";
            else if (item.CampoModificado == EmployeeHistoryFields.Position) column = "Cargo";
            else if (item.CampoModificado == EmployeeHistoryFields.Department) column = "IdDepartamento";
            else if (item.CampoModificado == EmployeeHistoryFields.Status) column = "Estado";
            else throw new InvalidOperationException("El campo de historial laboral no es compatible.");

            using (SQLiteCommand command = new SQLiteCommand("UPDATE Empleados SET " + column +
                " = @valor, FechaEfectivaLaboral = @efectiva WHERE IdEmpleado = @empleado;", connection, transaction))
            {
                object value = item.ValorNuevoTecnico ?? item.ValorNuevo;
                if (item.CampoModificado == EmployeeHistoryFields.BaseSalary)
                    value = decimal.Parse(Convert.ToString(value), System.Globalization.CultureInfo.InvariantCulture);
                else if (item.CampoModificado == EmployeeHistoryFields.Department)
                    value = int.Parse(Convert.ToString(value), System.Globalization.CultureInfo.InvariantCulture);
                command.Parameters.AddWithValue("@valor", value);
                command.Parameters.AddWithValue("@efectiva", item.FechaEfectiva.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@empleado", item.IdEmpleado);
                if (command.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se pudo aplicar el cambio laboral programado.");
            }

            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE HistorialEmpleado
                SET Aplicado = 1, FechaAplicacion = @fecha WHERE IdHistorial = @id AND Aplicado = 0;",
                connection, transaction))
            {
                command.Parameters.AddWithValue("@fecha", appliedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@id", item.IdHistorial);
                if (command.ExecuteNonQuery() != 1) throw new InvalidOperationException("El cambio laboral ya fue procesado.");
            }
        }

        private static HistorialEmpleado Map(SQLiteDataReader reader)
        {
            return new HistorialEmpleado
            {
                IdHistorial = Convert.ToInt32(reader["IdHistorial"]),
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                CodigoEmpleado = Convert.ToString(reader["CodigoEmpleado"]),
                EmpleadoNombre = Convert.ToString(reader["EmpleadoNombre"]),
                CampoModificado = Convert.ToString(reader["CampoModificado"]),
                ValorAnterior = Convert.ToString(reader["ValorAnterior"]),
                ValorNuevo = Convert.ToString(reader["ValorNuevo"]),
                ValorAnteriorTecnico = Convert.ToString(reader["ValorAnteriorTecnico"]),
                ValorNuevoTecnico = Convert.ToString(reader["ValorNuevoTecnico"]),
                FechaCambio = DateTime.Parse(Convert.ToString(reader["FechaCambio"])),
                FechaEfectiva = DateTime.Parse(Convert.ToString(reader["FechaEfectiva"])),
                UsuarioResponsable = Convert.ToString(reader["UsuarioResponsable"]),
                Motivo = Convert.ToString(reader["Motivo"]),
                Aplicado = Convert.ToInt32(reader["Aplicado"]) == 1,
                FechaAplicacion = reader["FechaAplicacion"] == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(reader["FechaAplicacion"]))
                    ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaAplicacion"]))
            };
        }
    }
}
