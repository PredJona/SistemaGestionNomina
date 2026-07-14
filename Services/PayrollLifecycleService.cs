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
        private readonly NominaRepository nominaRepository = new NominaRepository();
        private readonly ComprobanteRepository comprobanteRepository = new ComprobanteRepository();
        private readonly NominaVersionRepository versionRepository = new NominaVersionRepository();
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private readonly PayrollPeriodPolicyService periodPolicyService = new PayrollPeriodPolicyService();

        public int Confirmar(Nomina nominaCalculada, DateTime fechaInicio, DateTime fechaFin)
        {
            authorizationService.DemandPermission(Permissions.PayrollConfirm);

            if (nominaCalculada == null || nominaCalculada.Detalles.Count == 0)
                throw new InvalidOperationException("No hay una nómina calculada para confirmar.");

            if (!string.Equals(nominaCalculada.Estado, "Calculada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se puede confirmar una nómina en estado Calculada.");

            periodPolicyService.VerificarFechasAbiertas(fechaInicio, fechaFin);

            string usuario = SessionContext.Username;
            DateTime ahora = DateTime.Now;

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        PeriodoNomina periodo = new PeriodoNomina();
                        periodo.Nombre = nominaCalculada.PeriodoNombre;
                        periodo.FechaInicio = fechaInicio;
                        periodo.FechaFin = fechaFin;
                        periodo.Estado = "Confirmado";
                        periodo.Cerrado = true;
                        periodo.FechaCierre = ahora;
                        periodo.CerradoPor = usuario;

                        int idPeriodo;
                        using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO PeriodosNomina
                            (Nombre, FechaInicio, FechaFin, Estado, Cerrado, FechaCierre, CerradoPor)
                            VALUES (@nombre, @inicio, @fin, @estado, 1, @fecCierre, @usrCierre);
                            SELECT last_insert_rowid();", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@nombre", periodo.Nombre);
                            cmd.Parameters.AddWithValue("@inicio", periodo.FechaInicio.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@fin", periodo.FechaFin.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@estado", periodo.Estado);
                            cmd.Parameters.AddWithValue("@fecCierre", periodo.FechaCierre.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@usrCierre", periodo.CerradoPor);
                            idPeriodo = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        nominaCalculada.IdPeriodo = idPeriodo;
                        nominaCalculada.Estado = "Confirmada";
                        nominaCalculada.FechaConfirmacion = ahora;
                        nominaCalculada.ConfirmadaPor = usuario;

                        int idNomina;
                        using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Nominas
                            (IdPeriodo, FechaCalculo, TotalIngresos, TotalDeducciones, TotalNeto, Estado,
                             FechaConfirmacion, ConfirmadaPor)
                            VALUES (@periodo, @fecha, @ingresos, @deducciones, @neto, @estado,
                             @fecConf, @usrConf);
                            SELECT last_insert_rowid();", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@periodo", idPeriodo);
                            cmd.Parameters.AddWithValue("@fecha", nominaCalculada.FechaCalculo.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@ingresos", nominaCalculada.TotalIngresos);
                            cmd.Parameters.AddWithValue("@deducciones", nominaCalculada.TotalDeducciones);
                            cmd.Parameters.AddWithValue("@neto", nominaCalculada.TotalNeto);
                            cmd.Parameters.AddWithValue("@estado", nominaCalculada.Estado);
                            cmd.Parameters.AddWithValue("@fecConf", ahora.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@usrConf", usuario);
                            idNomina = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (NominaDetalle detalle in nominaCalculada.Detalles)
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO NominaDetalle
                                (IdNomina, IdEmpleado, SueldoBase, Bonos, HorasExtra, MontoHorasExtra,
                                 TotalIngresos, TotalDeducciones, NetoPagar)
                                VALUES (@nomina, @empleado, @sueldo, @bonos, @horas, @montoHoras,
                                 @ingresos, @deducciones, @neto);", connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@nomina", idNomina);
                                cmd.Parameters.AddWithValue("@empleado", detalle.IdEmpleado);
                                cmd.Parameters.AddWithValue("@sueldo", detalle.SueldoBase);
                                cmd.Parameters.AddWithValue("@bonos", detalle.Bonos);
                                cmd.Parameters.AddWithValue("@horas", detalle.HorasExtra);
                                cmd.Parameters.AddWithValue("@montoHoras", detalle.MontoHorasExtra);
                                cmd.Parameters.AddWithValue("@ingresos", detalle.TotalIngresos);
                                cmd.Parameters.AddWithValue("@deducciones", detalle.TotalDeducciones);
                                cmd.Parameters.AddWithValue("@neto", detalle.NetoPagar);
                                cmd.ExecuteNonQuery();
                            }

                            using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT OR IGNORE INTO Comprobantes
                                (IdNomina, IdEmpleado, NumeroComprobante, FechaGeneracion, RutaPdf)
                                VALUES (@nomina, @empleado, @numero, @fecha, @ruta);", connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@nomina", idNomina);
                                cmd.Parameters.AddWithValue("@empleado", detalle.IdEmpleado);
                                cmd.Parameters.AddWithValue("@numero", "COMP-" + idNomina.ToString("0000") + "-" + detalle.IdEmpleado.ToString("0000"));
                                cmd.Parameters.AddWithValue("@fecha", ahora.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@ruta", string.Empty);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        auditTrailService.RegistrarAccion("Nómina", "Confirmar",
                            "IdNomina=" + idNomina + ", Periodo=" + idPeriodo + ", Empleados=" + nominaCalculada.Detalles.Count);
                        auditTrailService.RegistrarAccion("Comprobantes", "Generar",
                            "IdNomina=" + idNomina + ", Cantidad=" + nominaCalculada.Detalles.Count);

                        return idNomina;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Pagar(int idNomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollPay);

            Nomina nomina = nominaRepository.GetById(idNomina);
            if (nomina == null)
                throw new InvalidOperationException("La nómina no existe.");

            if (!string.Equals(nomina.Estado, "Confirmada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se puede pagar una nómina en estado Confirmada.");

            string usuario = SessionContext.Username;
            DateTime ahora = DateTime.Now;

            nominaRepository.UpdateEstadoNomina(idNomina, "Pagada",
                "FechaPago", ahora,
                "PagadaPor", usuario);

            auditTrailService.RegistrarAccion("Nómina", "Pagar",
                "IdNomina=" + idNomina + ", Usuario=" + usuario);
        }

        public void Anular(int idNomina, string motivo)
        {
            authorizationService.DemandPermission(Permissions.PayrollAnnul);

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("Debe indicar un motivo de anulación.");

            Nomina nomina = nominaRepository.GetById(idNomina);
            if (nomina == null)
                throw new InvalidOperationException("La nómina no existe.");

            string estado = nomina.Estado ?? string.Empty;
            if (!string.Equals(estado, "Confirmada", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(estado, "Pagada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se puede anular una nómina Confirmada o Pagada.");

            string usuario = SessionContext.Username;
            DateTime ahora = DateTime.Now;

            nominaRepository.UpdateEstadoNomina(idNomina, "Anulada",
                "FechaAnulacion", ahora,
                "AnuladaPor", usuario,
                motivo);

            NominaVersion version = new NominaVersion
            {
                IdNominaOriginal = idNomina,
                IdNominaNueva = null,
                MotivoCambio = motivo,
                UsuarioResponsable = usuario,
                FechaCambio = ahora
            };
            versionRepository.Add(version);

            PeriodoNomina periodo = nominaRepository.GetPeriodoById(nomina.IdPeriodo);
            if (periodo != null && periodo.Cerrado)
            {
                nominaRepository.ReabrirPeriodo(nomina.IdPeriodo);
            }

            auditTrailService.RegistrarAccion("Nómina", "Anular",
                "IdNomina=" + idNomina + ", Motivo=" + motivo);
        }

        public int Recalcular(int idNominaAnulada, DateTime fechaInicio, DateTime fechaFin, Nomina nominaRecalculada)
        {
            authorizationService.DemandPermission(Permissions.PayrollRecalculate);

            Nomina nominaOriginal = nominaRepository.GetById(idNominaAnulada);
            if (nominaOriginal == null)
                throw new InvalidOperationException("La nómina original no existe.");

            if (!string.Equals(nominaOriginal.Estado, "Anulada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se puede recalcular a partir de una nómina anulada.");

            List<NominaVersion> versiones = versionRepository.GetByIdNominaOriginal(idNominaAnulada);
            bool tieneVersionPendiente = false;
            int idVersion = 0;
            for (int i = 0; i < versiones.Count; i++)
            {
                if (!versiones[i].IdNominaNueva.HasValue)
                {
                    tieneVersionPendiente = true;
                    idVersion = versiones[i].IdVersion;
                    break;
                }
            }

            if (!tieneVersionPendiente)
                throw new InvalidOperationException("No hay una versión pendiente. Debe anular la nómina primero.");

            int idNuevaNomina = Confirmar(nominaRecalculada, fechaInicio, fechaFin);

            versionRepository.UpdateIdNominaNueva(idVersion, idNuevaNomina);

            auditTrailService.RegistrarAccion("Nómina", "Recalcular",
                "IdNominaOriginal=" + idNominaAnulada + ", IdNominaNueva=" + idNuevaNomina);

            return idNuevaNomina;
        }

        public List<NominaVersion> ObtenerHistorial(int idNomina)
        {
            authorizationService.DemandPermission(Permissions.PayrollHistoryView);
            return versionRepository.GetHistorialChain(idNomina);
        }
    }
}
