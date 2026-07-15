using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Data
{
    public class UsuarioRepository
    {
        public Usuario GetByUsername(string username)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Usuarios WHERE NombreUsuario = @u AND Estado = 'Activo';", connection))
            {
                command.Parameters.AddWithValue("@u", username);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
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
                }
            }

            return null;
        }

        private static DateTime? ParseNullableDate(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(value))) return null;
            DateTime parsed;
            return DateTime.TryParse(Convert.ToString(value), out parsed) ? parsed : (DateTime?)null;
        }
    }

    public class DepartamentoRepository
    {
        public List<Departamento> GetAll()
        {
            List<Departamento> items = new List<Departamento>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT IdDepartamento, Nombre FROM Departamentos ORDER BY Nombre;", connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new Departamento
                    {
                        IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]),
                        Nombre = Convert.ToString(reader["Nombre"])
                    });
                }
            }

            return items;
        }
    }

    public class EmpleadoRepository
    {
        public List<Empleado> GetAll(string search, int? departmentId, string status)
        {
            return GetAll(search, departmentId, status, null);
        }

        public List<Empleado> GetAll(string search, int? departmentId, string status, int? scopeDepartmentId)
        {
            List<Empleado> items = new List<Empleado>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT e.*, d.Nombre AS DepartamentoNombre
                FROM Empleados e
                INNER JOIN Departamentos d ON d.IdDepartamento = e.IdDepartamento
                WHERE (@search = '' OR e.Codigo LIKE @like OR e.Nombre LIKE @like OR e.Apellido LIKE @like OR e.Cedula LIKE @like)
                  AND (@dep = 0 OR e.IdDepartamento = @dep)
                  AND (@scopeDep = 0 OR e.IdDepartamento = @scopeDep)
                  AND (@estado = '' OR e.Estado = @estado)
                ORDER BY e.Nombre, e.Apellido;", connection))
            {
                command.Parameters.AddWithValue("@search", search ?? string.Empty);
                command.Parameters.AddWithValue("@like", "%" + (search ?? string.Empty) + "%");
                command.Parameters.AddWithValue("@dep", departmentId.HasValue ? departmentId.Value : 0);
                command.Parameters.AddWithValue("@scopeDep", scopeDepartmentId.HasValue ? scopeDepartmentId.Value : 0);
                command.Parameters.AddWithValue("@estado", status ?? string.Empty);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapEmpleado(reader));
                    }
                }
            }

            return items;
        }

        public List<Empleado> GetActiveByDepartment(int? departmentId)
        {
            return GetAll(string.Empty, departmentId, "Activo");
        }

        public List<Empleado> GetActiveByDepartment(int? departmentId, int? scopeDepartmentId)
        {
            return GetAll(string.Empty, departmentId, "Activo", scopeDepartmentId);
        }

        public List<Empleado> GetAllForEffectiveDate(int? departmentId)
        {
            return GetAll(string.Empty, departmentId, string.Empty, null);
        }

        public Empleado GetById(int id)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                return GetById(connection, null, id);
            }
        }

        public Empleado GetById(SQLiteConnection connection, SQLiteTransaction transaction, int id)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT e.*, d.Nombre AS DepartamentoNombre
                FROM Empleados e INNER JOIN Departamentos d ON d.IdDepartamento = e.IdDepartamento
                WHERE e.IdEmpleado = @id;", connection, transaction))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapEmpleado(reader) : null;
                }
            }
        }

        public int? GetDepartmentId(int employeeId)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(
                "SELECT IdDepartamento FROM Empleados WHERE IdEmpleado = @id LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@id", employeeId);
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? (int?)null : Convert.ToInt32(value);
            }
        }

        public bool ExistsCode(string code, int ignoreId)
        {
            return Exists("Codigo", code, ignoreId);
        }

        public bool ExistsCedula(string cedula, int ignoreId)
        {
            return Exists("Cedula", cedula, ignoreId);
        }

        public int Add(Empleado empleado)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Empleados
                (Codigo, Nombre, Apellido, Cedula, Cargo, IdDepartamento, SalarioBase, Estado, FechaIngreso, FechaEfectivaLaboral)
                VALUES (@codigo, @nombre, @apellido, @cedula, @cargo, @dep, @salario, @estado, @fecha, @efectiva);
                SELECT last_insert_rowid();", connection))
            {
                FillEmpleadoParameters(command, empleado);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Empleado empleado)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                Update(connection, transaction, empleado);
                transaction.Commit();
            }
        }

        public void Update(SQLiteConnection connection, SQLiteTransaction transaction, Empleado empleado)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Empleados SET
                Codigo = @codigo, Nombre = @nombre, Apellido = @apellido, Cedula = @cedula,
                Cargo = @cargo, IdDepartamento = @dep, SalarioBase = @salario,
                Estado = @estado, FechaIngreso = @fecha, FechaEfectivaLaboral = @efectiva
                WHERE IdEmpleado = @id;", connection, transaction))
            {
                FillEmpleadoParameters(command, empleado);
                command.Parameters.AddWithValue("@id", empleado.IdEmpleado);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("No se pudo actualizar el empleado.");
            }
        }

        public string GetDepartmentName(int departmentId)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(
                "SELECT Nombre FROM Departamentos WHERE IdDepartamento = @id LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@id", departmentId);
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
            }
        }

        public void Deactivate(int id)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("UPDATE Empleados SET Estado = 'Inactivo' WHERE IdEmpleado = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private bool Exists(string field, string value, int ignoreId)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT COUNT(1) FROM Empleados WHERE " + field + " = @value AND IdEmpleado <> @id;", connection))
            {
                command.Parameters.AddWithValue("@value", value);
                command.Parameters.AddWithValue("@id", ignoreId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static Empleado MapEmpleado(SQLiteDataReader reader)
        {
            return new Empleado
            {
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                Codigo = Convert.ToString(reader["Codigo"]),
                Nombre = Convert.ToString(reader["Nombre"]),
                Apellido = Convert.ToString(reader["Apellido"]),
                Cedula = Convert.ToString(reader["Cedula"]),
                Cargo = Convert.ToString(reader["Cargo"]),
                IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]),
                DepartamentoNombre = Convert.ToString(reader["DepartamentoNombre"]),
                SalarioBase = Convert.ToDecimal(reader["SalarioBase"]),
                Estado = Convert.ToString(reader["Estado"]),
                FechaIngreso = DateTime.Parse(Convert.ToString(reader["FechaIngreso"])),
                FechaEfectivaLaboral = reader["FechaEfectivaLaboral"] == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(reader["FechaEfectivaLaboral"]))
                    ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaEfectivaLaboral"]))
            };
        }

        private static void FillEmpleadoParameters(SQLiteCommand command, Empleado empleado)
        {
            command.Parameters.AddWithValue("@codigo", empleado.Codigo);
            command.Parameters.AddWithValue("@nombre", empleado.Nombre);
            command.Parameters.AddWithValue("@apellido", empleado.Apellido);
            command.Parameters.AddWithValue("@cedula", empleado.Cedula);
            command.Parameters.AddWithValue("@cargo", empleado.Cargo);
            command.Parameters.AddWithValue("@dep", empleado.IdDepartamento);
            command.Parameters.AddWithValue("@salario", empleado.SalarioBase);
            command.Parameters.AddWithValue("@estado", empleado.Estado);
            command.Parameters.AddWithValue("@fecha", empleado.FechaIngreso.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@efectiva", (empleado.FechaEfectivaLaboral ?? empleado.FechaIngreso).ToString("yyyy-MM-dd"));
        }
    }

    public class AsistenciaRepository
    {
        public bool Exists(int employeeId, DateTime date)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT COUNT(1) FROM Asistencias
                WHERE IdEmpleado = @empleado AND Fecha = @fecha;", connection))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@fecha", date.ToString("yyyy-MM-dd"));
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public int Add(Asistencia asistencia)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Asistencias
                (IdEmpleado, Fecha, HoraEntrada, HoraSalida, HorasTrabajadas, Estado)
                VALUES (@empleado, @fecha, @entrada, @salida, @horas, @estado);
                SELECT last_insert_rowid();", connection))
            {
                command.Parameters.AddWithValue("@empleado", asistencia.IdEmpleado);
                command.Parameters.AddWithValue("@fecha", asistencia.Fecha.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@entrada", asistencia.HoraEntrada.HasValue ? asistencia.HoraEntrada.Value.ToString(@"hh\:mm") : null);
                command.Parameters.AddWithValue("@salida", asistencia.HoraSalida.HasValue ? asistencia.HoraSalida.Value.ToString(@"hh\:mm") : null);
                command.Parameters.AddWithValue("@horas", asistencia.HorasTrabajadas);
                command.Parameters.AddWithValue("@estado", asistencia.Estado);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<Asistencia> GetAll(DateTime? start, DateTime? end, int? employeeId, string status)
        {
            return GetAll(start, end, employeeId, status, null);
        }

        public List<Asistencia> GetAll(DateTime? start, DateTime? end, int? employeeId, string status,
            int? scopeDepartmentId)
        {
            List<Asistencia> items = new List<Asistencia>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT a.*, e.Nombre || ' ' || e.Apellido AS EmpleadoNombre
                FROM Asistencias a
                INNER JOIN Empleados e ON e.IdEmpleado = a.IdEmpleado
                WHERE (@inicio = '' OR a.Fecha >= @inicio)
                  AND (@fin = '' OR a.Fecha <= @fin)
                  AND (@empleado = 0 OR a.IdEmpleado = @empleado)
                  AND (@scopeDep = 0 OR e.IdDepartamento = @scopeDep)
                  AND (@estado = '' OR a.Estado = @estado)
                ORDER BY a.Fecha DESC, EmpleadoNombre;", connection))
            {
                command.Parameters.AddWithValue("@inicio", start.HasValue ? start.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@fin", end.HasValue ? end.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@empleado", employeeId.HasValue ? employeeId.Value : 0);
                command.Parameters.AddWithValue("@scopeDep", scopeDepartmentId.HasValue ? scopeDepartmentId.Value : 0);
                command.Parameters.AddWithValue("@estado", status ?? string.Empty);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapAsistencia(reader));
                    }
                }
            }

            return items;
        }

        public List<Asistencia> GetByEmployee(int employeeId, DateTime? start, DateTime? end, string status)
        {
            List<Asistencia> items = new List<Asistencia>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT a.*, e.Nombre || ' ' || e.Apellido AS EmpleadoNombre
                FROM Asistencias a
                INNER JOIN Empleados e ON e.IdEmpleado = a.IdEmpleado
                WHERE a.IdEmpleado = @empleado
                  AND (@inicio = '' OR a.Fecha >= @inicio)
                  AND (@fin = '' OR a.Fecha <= @fin)
                  AND (@estado = '' OR a.Estado = @estado)
                ORDER BY a.Fecha DESC;", connection))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@inicio", start.HasValue ? start.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@fin", end.HasValue ? end.Value.ToString("yyyy-MM-dd") : string.Empty);
                command.Parameters.AddWithValue("@estado", status ?? string.Empty);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapAsistencia(reader));
                    }
                }
            }

            return items;
        }

        public decimal GetExtraHours(int employeeId, DateTime start, DateTime end)
        {
            decimal total = 0;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT HorasTrabajadas FROM Asistencias
                WHERE IdEmpleado = @id AND Fecha BETWEEN @inicio AND @fin AND Estado IN ('Puntual','Tardanza');", connection))
            {
                command.Parameters.AddWithValue("@id", employeeId);
                command.Parameters.AddWithValue("@inicio", start.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", end.ToString("yyyy-MM-dd"));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal hours = Convert.ToDecimal(reader["HorasTrabajadas"]);
                        if (hours > 8)
                        {
                            total += hours - 8;
                        }
                    }
                }
            }

            return total;
        }

        private static Asistencia MapAsistencia(SQLiteDataReader reader)
        {
            string entrada = Convert.ToString(reader["HoraEntrada"]);
            string salida = Convert.ToString(reader["HoraSalida"]);
            return new Asistencia
            {
                IdAsistencia = Convert.ToInt32(reader["IdAsistencia"]),
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                EmpleadoNombre = Convert.ToString(reader["EmpleadoNombre"]),
                Fecha = DateTime.Parse(Convert.ToString(reader["Fecha"])),
                HoraEntrada = string.IsNullOrWhiteSpace(entrada) ? (TimeSpan?)null : TimeSpan.Parse(entrada),
                HoraSalida = string.IsNullOrWhiteSpace(salida) ? (TimeSpan?)null : TimeSpan.Parse(salida),
                HorasTrabajadas = Convert.ToDecimal(reader["HorasTrabajadas"]),
                Estado = Convert.ToString(reader["Estado"]),
                IdSolicitudAusencia = reader["IdSolicitudAusencia"] == DBNull.Value
                    ? (int?)null : Convert.ToInt32(reader["IdSolicitudAusencia"])
            };
        }
    }

    public class NominaRepository
    {
        public int CreatePeriodo(PeriodoNomina periodo)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO PeriodosNomina (Nombre, FechaInicio, FechaFin, Estado)
                VALUES (@nombre, @inicio, @fin, @estado); SELECT last_insert_rowid();", connection))
            {
                command.Parameters.AddWithValue("@nombre", periodo.Nombre);
                command.Parameters.AddWithValue("@inicio", periodo.FechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", periodo.FechaFin.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@estado", periodo.Estado);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int CreateNomina(Nomina nomina)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Nominas
                    (IdPeriodo, FechaCalculo, TotalIngresos, TotalDeducciones, TotalNeto, Estado)
                    VALUES (@periodo, @fecha, @ingresos, @deducciones, @neto, @estado);
                    SELECT last_insert_rowid();", connection, transaction))
                {
                    command.Parameters.AddWithValue("@periodo", nomina.IdPeriodo);
                    command.Parameters.AddWithValue("@fecha", nomina.FechaCalculo.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@ingresos", nomina.TotalIngresos);
                    command.Parameters.AddWithValue("@deducciones", nomina.TotalDeducciones);
                    command.Parameters.AddWithValue("@neto", nomina.TotalNeto);
                    command.Parameters.AddWithValue("@estado", nomina.Estado);
                    int idNomina = Convert.ToInt32(command.ExecuteScalar());

                    foreach (NominaDetalle detalle in nomina.Detalles)
                    {
                        using (SQLiteCommand detailCommand = new SQLiteCommand(@"INSERT INTO NominaDetalle
                            (IdNomina, IdEmpleado, SueldoBase, Bonos, HorasExtra, MontoHorasExtra, TotalIngresos, TotalDeducciones, NetoPagar)
                            VALUES (@nomina, @empleado, @sueldo, @bonos, @horas, @montoHoras, @ingresos, @deducciones, @neto);", connection, transaction))
                        {
                            detailCommand.Parameters.AddWithValue("@nomina", idNomina);
                            detailCommand.Parameters.AddWithValue("@empleado", detalle.IdEmpleado);
                            detailCommand.Parameters.AddWithValue("@sueldo", detalle.SueldoBase);
                            detailCommand.Parameters.AddWithValue("@bonos", detalle.Bonos);
                            detailCommand.Parameters.AddWithValue("@horas", detalle.HorasExtra);
                            detailCommand.Parameters.AddWithValue("@montoHoras", detalle.MontoHorasExtra);
                            detailCommand.Parameters.AddWithValue("@ingresos", detalle.TotalIngresos);
                            detailCommand.Parameters.AddWithValue("@deducciones", detalle.TotalDeducciones);
                            detailCommand.Parameters.AddWithValue("@neto", detalle.NetoPagar);
                            detailCommand.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return idNomina;
                }
            }
        }

        public List<Nomina> GetAll()
        {
            List<Nomina> items = new List<Nomina>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT n.*, p.Nombre AS PeriodoNombre
                FROM Nominas n INNER JOIN PeriodosNomina p ON p.IdPeriodo = n.IdPeriodo
                WHERE n.Estado <> 'Anulada'
                ORDER BY n.FechaCalculo DESC;", connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(MapNomina(reader));
                }
            }

            return items;
        }

        public List<NominaDetalle> GetDetalles(int idNomina)
        {
            List<NominaDetalle> items = new List<NominaDetalle>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT nd.*,
                    COALESCE(NULLIF(nd.CodigoEmpleadoSnapshot, ''), e.Codigo) AS Codigo,
                    COALESCE(NULLIF(nd.NombreEmpleadoSnapshot, ''), e.Nombre || ' ' || e.Apellido) AS EmpleadoNombre,
                    COALESCE(NULLIF(nd.CargoEmpleadoSnapshot, ''), e.Cargo) AS CargoEmpleado,
                    COALESCE(NULLIF(nd.DepartamentoSnapshot, ''), d.Nombre) AS Departamento
                FROM NominaDetalle nd
                INNER JOIN Empleados e ON e.IdEmpleado = nd.IdEmpleado
                INNER JOIN Departamentos d ON d.IdDepartamento = e.IdDepartamento
                WHERE nd.IdNomina = @id
                ORDER BY EmpleadoNombre;", connection))
            {
                command.Parameters.AddWithValue("@id", idNomina);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapDetalle(reader));
                    }
                }
            }

            return items;
        }

        public Nomina GetById(int idNomina)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT n.*, p.Nombre AS PeriodoNombre
                FROM Nominas n INNER JOIN PeriodosNomina p ON p.IdPeriodo = n.IdPeriodo
                WHERE n.IdNomina = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", idNomina);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapNomina(reader);
                }
            }
            return null;
        }

        public void UpdateEstadoNomina(int idNomina, string estado,
            string campoFecha, DateTime? fecha,
            string campoUsuario, string usuario,
            string motivo = null)
        {
            string sql = "UPDATE Nominas SET Estado = @estado";
            if (!string.IsNullOrWhiteSpace(campoFecha) && fecha.HasValue)
                sql += ", " + campoFecha + " = @fecha";
            if (!string.IsNullOrWhiteSpace(campoUsuario) && !string.IsNullOrWhiteSpace(usuario))
                sql += ", " + campoUsuario + " = @usuario";
            if (motivo != null)
                sql += ", MotivoAnulacion = @motivo";
            sql += " WHERE IdNomina = @id;";

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", idNomina);
                command.Parameters.AddWithValue("@estado", estado);
                if (fecha.HasValue) command.Parameters.AddWithValue("@fecha", fecha.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrWhiteSpace(usuario)) command.Parameters.AddWithValue("@usuario", usuario);
                if (motivo != null) command.Parameters.AddWithValue("@motivo", motivo);
                command.ExecuteNonQuery();
            }
        }

        public void CerrarPeriodo(int idPeriodo, string usuario)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE PeriodosNomina
                SET Cerrado = 1, FechaCierre = @fecha, CerradoPor = @usuario
                WHERE IdPeriodo = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", idPeriodo);
                command.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@usuario", usuario);
                command.ExecuteNonQuery();
            }
        }

        public void ReabrirPeriodo(int idPeriodo)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE PeriodosNomina
                SET Cerrado = 0, FechaCierre = NULL, CerradoPor = NULL
                WHERE IdPeriodo = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", idPeriodo);
                command.ExecuteNonQuery();
            }
        }

        public PeriodoNomina GetPeriodoById(int idPeriodo)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM PeriodosNomina WHERE IdPeriodo = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", idPeriodo);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PeriodoNomina
                        {
                            IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            FechaInicio = DateTime.Parse(Convert.ToString(reader["FechaInicio"])),
                            FechaFin = DateTime.Parse(Convert.ToString(reader["FechaFin"])),
                            Estado = Convert.ToString(reader["Estado"]),
                            Cerrado = Convert.ToInt32(reader["Cerrado"]) == 1,
                            FechaCierre = reader["FechaCierre"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaCierre"])),
                            CerradoPor = reader["CerradoPor"] == DBNull.Value ? null : Convert.ToString(reader["CerradoPor"])
                        };
                    }
                }
            }
            return null;
        }

        public PeriodoNomina GetPeriodoByFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT * FROM PeriodosNomina
                WHERE FechaInicio <= @fin AND FechaFin >= @inicio
                ORDER BY Cerrado DESC, FechaInicio LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@inicio", fechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", fechaFin.ToString("yyyy-MM-dd"));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PeriodoNomina
                        {
                            IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            FechaInicio = DateTime.Parse(Convert.ToString(reader["FechaInicio"])),
                            FechaFin = DateTime.Parse(Convert.ToString(reader["FechaFin"])),
                            Estado = Convert.ToString(reader["Estado"]),
                            Cerrado = Convert.ToInt32(reader["Cerrado"]) == 1
                        };
                    }
                }
            }
            return null;
        }

        public PeriodoNomina GetOverlappingProtectedPeriod(DateTime fechaInicio, DateTime fechaFin)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT * FROM PeriodosNomina
                WHERE FechaInicio <= @fin AND FechaFin >= @inicio
                  AND (Cerrado = 1 OR Estado IN ('Confirmado','Pagado'))
                ORDER BY FechaInicio LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@inicio", fechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", fechaFin.ToString("yyyy-MM-dd"));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapPeriodo(reader);
                }
            }
            return null;
        }

        public bool HasClosedPeriodAffectedByEmployeeChange(DateTime effectiveDate)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                return HasClosedPeriodAffectedByEmployeeChange(connection, null, effectiveDate);
            }
        }

        public bool HasClosedPeriodAffectedByEmployeeChange(SQLiteConnection connection,
            SQLiteTransaction transaction, DateTime effectiveDate)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT COUNT(1) FROM PeriodosNomina
                WHERE Cerrado = 1 AND FechaFin >= @efectiva;", connection, transaction))
            {
                command.Parameters.AddWithValue("@efectiva", effectiveDate.ToString("yyyy-MM-dd"));
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private static PeriodoNomina MapPeriodo(SQLiteDataReader reader)
        {
            return new PeriodoNomina
            {
                IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                Nombre = Convert.ToString(reader["Nombre"]),
                FechaInicio = DateTime.Parse(Convert.ToString(reader["FechaInicio"])),
                FechaFin = DateTime.Parse(Convert.ToString(reader["FechaFin"])),
                Estado = Convert.ToString(reader["Estado"]),
                Cerrado = Convert.ToInt32(reader["Cerrado"]) == 1,
                FechaCierre = reader["FechaCierre"] == DBNull.Value ? (DateTime?)null :
                    DateTime.Parse(Convert.ToString(reader["FechaCierre"])),
                CerradoPor = reader["CerradoPor"] == DBNull.Value ? null : Convert.ToString(reader["CerradoPor"])
            };
        }

        public int CreateNominaEnTransaccion(SQLiteConnection connection, SQLiteTransaction transaction, Nomina nomina)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Nominas
                (IdPeriodo, FechaCalculo, TotalIngresos, TotalDeducciones, TotalNeto, Estado,
                 FechaConfirmacion, ConfirmadaPor)
                VALUES (@periodo, @fecha, @ingresos, @deducciones, @neto, @estado, @fecConf, @usrConf);
                SELECT last_insert_rowid();", connection, transaction))
            {
                command.Parameters.AddWithValue("@periodo", nomina.IdPeriodo);
                command.Parameters.AddWithValue("@fecha", nomina.FechaCalculo.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@ingresos", nomina.TotalIngresos);
                command.Parameters.AddWithValue("@deducciones", nomina.TotalDeducciones);
                command.Parameters.AddWithValue("@neto", nomina.TotalNeto);
                command.Parameters.AddWithValue("@estado", nomina.Estado);
                command.Parameters.AddWithValue("@fecConf", nomina.FechaConfirmacion?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@usrConf", nomina.ConfirmadaPor ?? string.Empty);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static Nomina MapNomina(SQLiteDataReader reader)
        {
            return new Nomina
            {
                IdNomina = Convert.ToInt32(reader["IdNomina"]),
                IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                PeriodoNombre = Convert.ToString(reader["PeriodoNombre"]),
                FechaCalculo = DateTime.Parse(Convert.ToString(reader["FechaCalculo"])),
                TotalIngresos = Convert.ToDecimal(reader["TotalIngresos"]),
                TotalDeducciones = Convert.ToDecimal(reader["TotalDeducciones"]),
                TotalNeto = Convert.ToDecimal(reader["TotalNeto"]),
                Estado = Convert.ToString(reader["Estado"]),
                FechaConfirmacion = reader["FechaConfirmacion"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaConfirmacion"])),
                ConfirmadaPor = reader["ConfirmadaPor"] == DBNull.Value ? null : Convert.ToString(reader["ConfirmadaPor"]),
                FechaPago = reader["FechaPago"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaPago"])),
                PagadaPor = reader["PagadaPor"] == DBNull.Value ? null : Convert.ToString(reader["PagadaPor"]),
                FechaAnulacion = reader["FechaAnulacion"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(Convert.ToString(reader["FechaAnulacion"])),
                AnuladaPor = reader["AnuladaPor"] == DBNull.Value ? null : Convert.ToString(reader["AnuladaPor"]),
                MotivoAnulacion = reader["MotivoAnulacion"] == DBNull.Value ? null : Convert.ToString(reader["MotivoAnulacion"])
            };
        }

        private static NominaDetalle MapDetalle(SQLiteDataReader reader)
        {
            return new NominaDetalle
            {
                IdDetalle = Convert.ToInt32(reader["IdDetalle"]),
                IdNomina = Convert.ToInt32(reader["IdNomina"]),
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                CodigoEmpleado = Convert.ToString(reader["Codigo"]),
                EmpleadoNombre = Convert.ToString(reader["EmpleadoNombre"]),
                CargoEmpleado = Convert.ToString(reader["CargoEmpleado"]),
                Departamento = Convert.ToString(reader["Departamento"]),
                SueldoBase = Convert.ToDecimal(reader["SueldoBase"]),
                Bonos = Convert.ToDecimal(reader["Bonos"]),
                HorasExtra = Convert.ToDecimal(reader["HorasExtra"]),
                MontoHorasExtra = Convert.ToDecimal(reader["MontoHorasExtra"]),
                TotalIngresos = Convert.ToDecimal(reader["TotalIngresos"]),
                TotalDeducciones = Convert.ToDecimal(reader["TotalDeducciones"]),
                NetoPagar = Convert.ToDecimal(reader["NetoPagar"])
            };
        }
    }

    public class NominaVersionRepository
    {
        public void Add(NominaVersion version)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO NominaVersiones
                (IdNominaOriginal, IdNominaNueva, MotivoCambio, UsuarioResponsable, FechaCambio)
                VALUES (@original, @nueva, @motivo, @usuario, @fecha);", connection))
            {
                command.Parameters.AddWithValue("@original", version.IdNominaOriginal);
                command.Parameters.AddWithValue("@nueva", version.IdNominaNueva.HasValue ? (object)version.IdNominaNueva.Value : DBNull.Value);
                command.Parameters.AddWithValue("@motivo", version.MotivoCambio);
                command.Parameters.AddWithValue("@usuario", version.UsuarioResponsable);
                command.Parameters.AddWithValue("@fecha", version.FechaCambio.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        public List<NominaVersion> GetByIdNominaOriginal(int idNominaOriginal)
        {
            List<NominaVersion> items = new List<NominaVersion>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT * FROM NominaVersiones
                WHERE IdNominaOriginal = @id ORDER BY FechaCambio DESC;", connection))
            {
                command.Parameters.AddWithValue("@id", idNominaOriginal);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new NominaVersion
                        {
                            IdVersion = Convert.ToInt32(reader["IdVersion"]),
                            IdNominaOriginal = Convert.ToInt32(reader["IdNominaOriginal"]),
                            IdNominaNueva = reader["IdNominaNueva"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdNominaNueva"]),
                            MotivoCambio = Convert.ToString(reader["MotivoCambio"]),
                            UsuarioResponsable = Convert.ToString(reader["UsuarioResponsable"]),
                            FechaCambio = DateTime.Parse(Convert.ToString(reader["FechaCambio"]))
                        });
                    }
                }
            }
            return items;
        }

        public void UpdateIdNominaNueva(int idVersion, int idNominaNueva)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE NominaVersiones
                SET IdNominaNueva = @nueva WHERE IdVersion = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", idVersion);
                command.Parameters.AddWithValue("@nueva", idNominaNueva);
                command.ExecuteNonQuery();
            }
        }

        public List<NominaVersion> GetHistorialChain(int idNomina)
        {
            List<NominaVersion> result = new List<NominaVersion>();
            int? currentId = idNomina;
            while (currentId.HasValue)
            {
                NominaVersion version = GetByNominaNueva(currentId.Value);
                if (version == null)
                    break;
                result.Add(version);
                currentId = version.IdNominaOriginal;
            }
            return result;
        }

        private NominaVersion GetByNominaNueva(int idNominaNueva)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT * FROM NominaVersiones
                WHERE IdNominaNueva = @id LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@id", idNominaNueva);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NominaVersion
                        {
                            IdVersion = Convert.ToInt32(reader["IdVersion"]),
                            IdNominaOriginal = Convert.ToInt32(reader["IdNominaOriginal"]),
                            IdNominaNueva = reader["IdNominaNueva"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdNominaNueva"]),
                            MotivoCambio = Convert.ToString(reader["MotivoCambio"]),
                            UsuarioResponsable = Convert.ToString(reader["UsuarioResponsable"]),
                            FechaCambio = DateTime.Parse(Convert.ToString(reader["FechaCambio"]))
                        };
                    }
                }
            }
            return null;
        }
    }

    public class ComprobanteRepository
    {
        private const string ComprobanteSelect = @"SELECT c.*,
                   COALESCE(NULLIF(nd.CodigoEmpleadoSnapshot, ''), e.Codigo) AS Codigo,
                   COALESCE(NULLIF(nd.NombreEmpleadoSnapshot, ''), e.Nombre || ' ' || e.Apellido) AS EmpleadoNombre,
                   p.Nombre AS PeriodoNombre, nd.TotalIngresos, nd.TotalDeducciones, nd.NetoPagar
            FROM Comprobantes c
            INNER JOIN Empleados e ON e.IdEmpleado = c.IdEmpleado
            INNER JOIN Nominas n ON n.IdNomina = c.IdNomina
            INNER JOIN PeriodosNomina p ON p.IdPeriodo = n.IdPeriodo
            INNER JOIN NominaDetalle nd ON nd.IdNomina = c.IdNomina AND nd.IdEmpleado = c.IdEmpleado ";

        public void Add(Comprobante comprobante)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT OR IGNORE INTO Comprobantes
                (IdNomina, IdEmpleado, NumeroComprobante, FechaGeneracion, RutaPdf)
                VALUES (@nomina, @empleado, @numero, @fecha, @ruta);", connection))
            {
                command.Parameters.AddWithValue("@nomina", comprobante.IdNomina);
                command.Parameters.AddWithValue("@empleado", comprobante.IdEmpleado);
                command.Parameters.AddWithValue("@numero", comprobante.NumeroComprobante);
                command.Parameters.AddWithValue("@fecha", comprobante.FechaGeneracion.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@ruta", comprobante.RutaPdf ?? string.Empty);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateRutaPdf(int idComprobante, string ruta)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("UPDATE Comprobantes SET RutaPdf = @ruta WHERE IdComprobante = @id;", connection))
            {
                command.Parameters.AddWithValue("@ruta", ruta ?? string.Empty);
                command.Parameters.AddWithValue("@id", idComprobante);
                command.ExecuteNonQuery();
            }
        }

        public bool UpdateRutaPdfForEmployee(int idComprobante, int employeeId, string ruta)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Comprobantes SET RutaPdf = @ruta
                WHERE IdComprobante = @id AND IdEmpleado = @empleado;", connection))
            {
                command.Parameters.AddWithValue("@ruta", ruta ?? string.Empty);
                command.Parameters.AddWithValue("@id", idComprobante);
                command.Parameters.AddWithValue("@empleado", employeeId);
                return command.ExecuteNonQuery() == 1;
            }
        }

        public List<Comprobante> GetAll(string search)
        {
            List<Comprobante> items = new List<Comprobante>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(ComprobanteSelect + @"
                WHERE (@search = '' OR c.NumeroComprobante LIKE @like OR e.Nombre LIKE @like OR e.Apellido LIKE @like OR p.Nombre LIKE @like)
                ORDER BY c.FechaGeneracion DESC;", connection))
            {
                command.Parameters.AddWithValue("@search", search ?? string.Empty);
                command.Parameters.AddWithValue("@like", "%" + (search ?? string.Empty) + "%");
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapComprobante(reader));
                    }
                }
            }

            return items;
        }

        public Comprobante GetById(int id)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(ComprobanteSelect +
                " WHERE c.IdComprobante = @id LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapComprobante(reader) : null;
                }
            }
        }

        public List<Comprobante> GetByEmployee(int employeeId, string search)
        {
            List<Comprobante> items = new List<Comprobante>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(ComprobanteSelect + @"
                WHERE c.IdEmpleado = @empleado
                  AND (@search = '' OR c.NumeroComprobante LIKE @like OR p.Nombre LIKE @like)
                ORDER BY c.FechaGeneracion DESC;", connection))
            {
                command.Parameters.AddWithValue("@empleado", employeeId);
                command.Parameters.AddWithValue("@search", search ?? string.Empty);
                command.Parameters.AddWithValue("@like", "%" + (search ?? string.Empty) + "%");
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapComprobante(reader));
                    }
                }
            }

            return items;
        }

        public Comprobante GetByIdAndEmployee(int payslipId, int employeeId)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(ComprobanteSelect + @"
                WHERE c.IdComprobante = @id AND c.IdEmpleado = @empleado LIMIT 1;", connection))
            {
                command.Parameters.AddWithValue("@id", payslipId);
                command.Parameters.AddWithValue("@empleado", employeeId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapComprobante(reader) : null;
                }
            }
        }

        private static Comprobante MapComprobante(SQLiteDataReader reader)
        {
            return new Comprobante
            {
                IdComprobante = Convert.ToInt32(reader["IdComprobante"]),
                IdNomina = Convert.ToInt32(reader["IdNomina"]),
                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                NumeroComprobante = Convert.ToString(reader["NumeroComprobante"]),
                FechaGeneracion = DateTime.Parse(Convert.ToString(reader["FechaGeneracion"])),
                RutaPdf = Convert.ToString(reader["RutaPdf"]),
                CodigoEmpleado = Convert.ToString(reader["Codigo"]),
                EmpleadoNombre = Convert.ToString(reader["EmpleadoNombre"]),
                PeriodoNombre = Convert.ToString(reader["PeriodoNombre"]),
                TotalIngresos = Convert.ToDecimal(reader["TotalIngresos"]),
                TotalDeducciones = Convert.ToDecimal(reader["TotalDeducciones"]),
                NetoPagar = Convert.ToDecimal(reader["NetoPagar"])
            };
        }
    }

    public class ConfiguracionRepository
    {
        public List<ConfiguracionNomina> GetAll()
        {
            List<ConfiguracionNomina> items = new List<ConfiguracionNomina>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ConfiguracionNomina ORDER BY NombreParametro;", connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new ConfiguracionNomina
                    {
                        IdConfiguracion = Convert.ToInt32(reader["IdConfiguracion"]),
                        NombreParametro = Convert.ToString(reader["NombreParametro"]),
                        Valor = Convert.ToDecimal(reader["Valor"]),
                        Descripcion = Convert.ToString(reader["Descripcion"])
                    });
                }
            }

            return items;
        }

        public decimal GetValue(string name, decimal fallback)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT Valor FROM ConfiguracionNomina WHERE NombreParametro = @n;", connection))
            {
                command.Parameters.AddWithValue("@n", name);
                object result = command.ExecuteScalar();
                return result == null || result == DBNull.Value ? fallback : Convert.ToDecimal(result);
            }
        }

        public void Save(string name, decimal value, string description)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO ConfiguracionNomina (NombreParametro, Valor, Descripcion)
                VALUES (@n, @v, @d)
                ON CONFLICT(NombreParametro) DO UPDATE SET Valor = excluded.Valor, Descripcion = excluded.Descripcion;", connection))
            {
                command.Parameters.AddWithValue("@n", name);
                command.Parameters.AddWithValue("@v", value);
                command.Parameters.AddWithValue("@d", description);
                command.ExecuteNonQuery();
            }
        }
    }

    public class ReporteRepository
    {
        public int Add(ReporteGenerado reporte)
        {
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO ReportesGenerados
                (NombreReporte, Tipo, GeneradoPor, FechaGeneracion, RutaArchivo)
                VALUES (@nombre, @tipo, @por, @fecha, @ruta); SELECT last_insert_rowid();", connection))
            {
                command.Parameters.AddWithValue("@nombre", reporte.NombreReporte);
                command.Parameters.AddWithValue("@tipo", reporte.Tipo);
                command.Parameters.AddWithValue("@por", reporte.GeneradoPor);
                command.Parameters.AddWithValue("@fecha", reporte.FechaGeneracion.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@ruta", reporte.RutaArchivo ?? string.Empty);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<ReporteGenerado> GetAll()
        {
            List<ReporteGenerado> items = new List<ReporteGenerado>();
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ReportesGenerados ORDER BY FechaGeneracion DESC;", connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new ReporteGenerado
                    {
                        IdReporte = Convert.ToInt32(reader["IdReporte"]),
                        NombreReporte = Convert.ToString(reader["NombreReporte"]),
                        Tipo = Convert.ToString(reader["Tipo"]),
                        GeneradoPor = Convert.ToString(reader["GeneradoPor"]),
                        FechaGeneracion = DateTime.Parse(Convert.ToString(reader["FechaGeneracion"])),
                        RutaArchivo = Convert.ToString(reader["RutaArchivo"])
                    });
                }
            }

            return items;
        }
    }
}
