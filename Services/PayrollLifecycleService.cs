using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public class PayrollLifecycleService
    {
        private readonly NominaVersionRepository versionRepository = new NominaVersionRepository();
        private readonly AuditRepository auditRepository = new AuditRepository();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly PayrollPeriodPolicyService periodPolicyService = new PayrollPeriodPolicyService();

        public int Confirmar(Nomina nominaCalculada, DateTime fechaInicio, DateTime fechaFin)
        {
            authorizationService.DemandPermission(Permissions.PayrollConfirm);
            ValidateCalculatedPayroll(nominaCalculada);
            periodPolicyService.VerificarFechasAbiertas(fechaInicio, fechaFin);

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int idNomina = CreateConfirmedPayroll(connection, transaction, nominaCalculada,
                        fechaInicio.Date, fechaFin.Date, CurrentUsername(), DateTime.Now);
                    transaction.Commit();
                    return idNomina;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Pagar(int idNomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollPay);
            string user = CurrentUsername();
            DateTime now = DateTime.Now;

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    Nomina payroll = GetPayroll(connection, transaction, idNomina);
                    if (payroll == null) throw new InvalidOperationException("La nómina no existe.");
                    PayrollStateMachine.DemandTransition(payroll.Estado, PayrollStates.Paid);

                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Nominas
                        SET Estado = @nuevo, FechaPago = @fecha, PagadaPor = @usuario
                        WHERE IdNomina = @id AND Estado = @anterior;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@nuevo", PayrollStates.Paid);
                        command.Parameters.AddWithValue("@fecha", now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@usuario", user);
                        command.Parameters.AddWithValue("@id", idNomina);
                        command.Parameters.AddWithValue("@anterior", PayrollStates.Confirmed);
                        if (command.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("La nómina cambió de estado antes de registrar el pago.");
                    }
                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE PeriodosNomina
                        SET Estado = @estado WHERE IdPeriodo = @id;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@estado", PayrollPeriodStates.Paid);
                        command.Parameters.AddWithValue("@id", payroll.IdPeriodo);
                        command.ExecuteNonQuery();
                    }
                    AddAudit(connection, transaction, user, "Nómina", "Pagar", "IdNomina=" + idNomina, now);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Anular(int idNomina, string motivo)
        {
            authorizationService.DemandPermission(Permissions.PayrollAnnul);
            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("Debe indicar un motivo de anulación.");

            string user = CurrentUsername();
            DateTime now = DateTime.Now;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    Nomina payroll = GetPayroll(connection, transaction, idNomina);
                    if (payroll == null) throw new InvalidOperationException("La nómina no existe.");
                    PayrollStateMachine.DemandTransition(payroll.Estado, PayrollStates.Annulled);

                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE Nominas
                        SET Estado = @nuevo, FechaAnulacion = @fecha, AnuladaPor = @usuario,
                            MotivoAnulacion = @motivo
                        WHERE IdNomina = @id AND Estado = @anterior;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@nuevo", PayrollStates.Annulled);
                        command.Parameters.AddWithValue("@fecha", now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@usuario", user);
                        command.Parameters.AddWithValue("@motivo", motivo.Trim());
                        command.Parameters.AddWithValue("@id", idNomina);
                        command.Parameters.AddWithValue("@anterior", payroll.Estado);
                        if (command.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("La nómina cambió de estado antes de anularse.");
                    }
                    using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO NominaVersiones
                        (IdNominaOriginal, IdNominaNueva, MotivoCambio, UsuarioResponsable, FechaCambio)
                        VALUES (@original, NULL, @motivo, @usuario, @fecha);", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@original", idNomina);
                        command.Parameters.AddWithValue("@motivo", motivo.Trim());
                        command.Parameters.AddWithValue("@usuario", user);
                        command.Parameters.AddWithValue("@fecha", now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.ExecuteNonQuery();
                    }
                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE PeriodosNomina
                        SET Estado = @estado, Cerrado = 0, FechaCierre = NULL, CerradoPor = NULL
                        WHERE IdPeriodo = @id;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@estado", PayrollPeriodStates.Reopened);
                        command.Parameters.AddWithValue("@id", payroll.IdPeriodo);
                        command.ExecuteNonQuery();
                    }
                    AddAudit(connection, transaction, user, "Nómina", "Anular",
                        "IdNomina=" + idNomina + ", Motivo=" + motivo.Trim(), now);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int Recalcular(int idNominaAnulada, DateTime fechaInicio, DateTime fechaFin,
            Nomina nominaRecalculada)
        {
            authorizationService.DemandPermission(Permissions.PayrollRecalculate);
            ValidateCalculatedPayroll(nominaRecalculada);
            periodPolicyService.VerificarFechasAbiertas(fechaInicio, fechaFin);

            string user = CurrentUsername();
            DateTime now = DateTime.Now;
            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    Nomina original = GetPayroll(connection, transaction, idNominaAnulada);
                    if (original == null) throw new InvalidOperationException("La nómina original no existe.");
                    if (!string.Equals(original.Estado, PayrollStates.Annulled, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Solo se puede recalcular a partir de una nómina anulada.");

                    int pendingVersionId = GetPendingVersionId(connection, transaction, idNominaAnulada);
                    if (pendingVersionId <= 0)
                        throw new InvalidOperationException("No hay una versión pendiente para la nómina anulada.");

                    int newPayrollId = CreateConfirmedPayroll(connection, transaction, nominaRecalculada,
                        fechaInicio.Date, fechaFin.Date, user, now);

                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE NominaVersiones
                        SET IdNominaNueva = @nueva
                        WHERE IdVersion = @id AND IdNominaNueva IS NULL;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@nueva", newPayrollId);
                        command.Parameters.AddWithValue("@id", pendingVersionId);
                        if (command.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("No se pudo vincular la nueva versión de nómina.");
                    }

                    AddAudit(connection, transaction, user, "Nómina", "Recalcular",
                        "IdNominaOriginal=" + idNominaAnulada + ", IdNominaNueva=" + newPayrollId, now);
                    transaction.Commit();
                    return newPayrollId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<NominaVersion> ObtenerHistorial(int idNomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollHistoryView);
            return versionRepository.GetHistorialChain(idNomina);
        }

        private int CreateConfirmedPayroll(SQLiteConnection connection, SQLiteTransaction transaction,
            Nomina calculated, DateTime start, DateTime end, string user, DateTime now)
        {
            int periodId;
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO PeriodosNomina
                (Nombre, FechaInicio, FechaFin, Estado, Cerrado, FechaCierre, CerradoPor)
                VALUES (@nombre, @inicio, @fin, @estado, 1, @cierre, @usuario);
                SELECT last_insert_rowid();", connection, transaction))
            {
                command.Parameters.AddWithValue("@nombre", calculated.PeriodoNombre);
                command.Parameters.AddWithValue("@inicio", start.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", end.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@estado", PayrollPeriodStates.Confirmed);
                command.Parameters.AddWithValue("@cierre", now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@usuario", user);
                periodId = Convert.ToInt32(command.ExecuteScalar());
            }

            int payrollId;
            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Nominas
                (IdPeriodo, FechaCalculo, TotalIngresos, TotalDeducciones, TotalNeto, Estado,
                 FechaConfirmacion, ConfirmadaPor)
                VALUES (@periodo, @fecha, @ingresos, @deducciones, @neto, @estado,
                 @confirmacion, @usuario);
                SELECT last_insert_rowid();", connection, transaction))
            {
                command.Parameters.AddWithValue("@periodo", periodId);
                command.Parameters.AddWithValue("@fecha", calculated.FechaCalculo.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@ingresos", calculated.TotalIngresos);
                command.Parameters.AddWithValue("@deducciones", calculated.TotalDeducciones);
                command.Parameters.AddWithValue("@neto", calculated.TotalNeto);
                command.Parameters.AddWithValue("@estado", PayrollStates.Confirmed);
                command.Parameters.AddWithValue("@confirmacion", now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@usuario", user);
                payrollId = Convert.ToInt32(command.ExecuteScalar());
            }

            for (int i = 0; i < calculated.Detalles.Count; i++)
            {
                NominaDetalle detail = calculated.Detalles[i];
                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO NominaDetalle
                    (IdNomina, IdEmpleado, SueldoBase, Bonos, HorasExtra, MontoHorasExtra,
                     TotalIngresos, TotalDeducciones, NetoPagar, CodigoEmpleadoSnapshot,
                     NombreEmpleadoSnapshot, CargoEmpleadoSnapshot, DepartamentoSnapshot)
                    VALUES (@nomina, @empleado, @sueldo, @bonos, @horas, @montoHoras,
                     @ingresos, @deducciones, @neto, @codigo, @nombre, @cargo, @departamento);",
                    connection, transaction))
                {
                    command.Parameters.AddWithValue("@nomina", payrollId);
                    command.Parameters.AddWithValue("@empleado", detail.IdEmpleado);
                    command.Parameters.AddWithValue("@sueldo", detail.SueldoBase);
                    command.Parameters.AddWithValue("@bonos", detail.Bonos);
                    command.Parameters.AddWithValue("@horas", detail.HorasExtra);
                    command.Parameters.AddWithValue("@montoHoras", detail.MontoHorasExtra);
                    command.Parameters.AddWithValue("@ingresos", detail.TotalIngresos);
                    command.Parameters.AddWithValue("@deducciones", detail.TotalDeducciones);
                    command.Parameters.AddWithValue("@neto", detail.NetoPagar);
                    command.Parameters.AddWithValue("@codigo", detail.CodigoEmpleado ?? string.Empty);
                    command.Parameters.AddWithValue("@nombre", detail.EmpleadoNombre ?? string.Empty);
                    command.Parameters.AddWithValue("@cargo", detail.CargoEmpleado ?? string.Empty);
                    command.Parameters.AddWithValue("@departamento", detail.Departamento ?? string.Empty);
                    command.ExecuteNonQuery();
                }
                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO Comprobantes
                    (IdNomina, IdEmpleado, NumeroComprobante, FechaGeneracion, RutaPdf)
                    VALUES (@nomina, @empleado, @numero, @fecha, '');", connection, transaction))
                {
                    command.Parameters.AddWithValue("@nomina", payrollId);
                    command.Parameters.AddWithValue("@empleado", detail.IdEmpleado);
                    command.Parameters.AddWithValue("@numero", "COMP-" + payrollId.ToString("0000") + "-" + detail.IdEmpleado.ToString("0000"));
                    command.Parameters.AddWithValue("@fecha", now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }

            AddAudit(connection, transaction, user, "Nómina", "Confirmar",
                "IdNomina=" + payrollId + ", Periodo=" + periodId + ", Empleados=" + calculated.Detalles.Count, now);
            AddAudit(connection, transaction, user, "Comprobantes", "Generar",
                "IdNomina=" + payrollId + ", Cantidad=" + calculated.Detalles.Count, now);
            return payrollId;
        }

        private static Nomina GetPayroll(SQLiteConnection connection, SQLiteTransaction transaction, int payrollId)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT n.*, p.Nombre AS PeriodoNombre
                FROM Nominas n INNER JOIN PeriodosNomina p ON p.IdPeriodo = n.IdPeriodo
                WHERE n.IdNomina = @id LIMIT 1;", connection, transaction))
            {
                command.Parameters.AddWithValue("@id", payrollId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new Nomina
                    {
                        IdNomina = Convert.ToInt32(reader["IdNomina"]),
                        IdPeriodo = Convert.ToInt32(reader["IdPeriodo"]),
                        PeriodoNombre = Convert.ToString(reader["PeriodoNombre"]),
                        FechaCalculo = DateTime.Parse(Convert.ToString(reader["FechaCalculo"])),
                        TotalIngresos = Convert.ToDecimal(reader["TotalIngresos"]),
                        TotalDeducciones = Convert.ToDecimal(reader["TotalDeducciones"]),
                        TotalNeto = Convert.ToDecimal(reader["TotalNeto"]),
                        Estado = Convert.ToString(reader["Estado"])
                    };
                }
            }
        }

        private static int GetPendingVersionId(SQLiteConnection connection, SQLiteTransaction transaction,
            int originalPayrollId)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"SELECT IdVersion FROM NominaVersiones
                WHERE IdNominaOriginal = @id AND IdNominaNueva IS NULL
                ORDER BY IdVersion DESC LIMIT 1;", connection, transaction))
            {
                command.Parameters.AddWithValue("@id", originalPayrollId);
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value);
            }
        }

        private void AddAudit(SQLiteConnection connection, SQLiteTransaction transaction, string user,
            string module, string action, string detail, DateTime date)
        {
            auditRepository.Add(connection, transaction, new AuditRecord
            {
                Usuario = user,
                Modulo = module,
                Accion = action,
                Detalle = detail,
                Fecha = date
            });
        }

        private static void ValidateCalculatedPayroll(Nomina payroll)
        {
            if (payroll == null || payroll.Detalles == null || payroll.Detalles.Count == 0)
                throw new InvalidOperationException("No hay una nómina calculada para confirmar.");
            PayrollStateMachine.DemandTransition(payroll.Estado, PayrollStates.Confirmed);
        }

        private static string CurrentUsername()
        {
            if (!SessionContext.IsAuthenticated || string.IsNullOrWhiteSpace(SessionContext.Username))
                throw new UnauthorizedAccessException("Debe iniciar sesión para gestionar la nómina.");
            return SessionContext.Username;
        }
    }
}
