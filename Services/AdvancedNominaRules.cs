using System;
using System.Data.SQLite;
using SistemaGestionNomina.Data;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Contiene reglas avanzadas opcionales de nómina.
    /// </summary>
    public class AdvancedNominaRules
    {
        /// <summary>
        /// Calcula ajustes especiales no incluidos en la nómina académica básica.
        /// </summary>
        public decimal CalcularAjusteEspecial(int idEmpleado, decimal ingresoBase)
        {
            DateTime today = DateTime.Today;
            DateTime start = new DateTime(today.Year, today.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            return CalcularAjusteEspecial(idEmpleado, ingresoBase, start, end);
        }

        /// <summary>
        /// Calcula ajuste por antiguedad, asistencia perfecta y descuentos disciplinarios del periodo.
        /// </summary>
        public decimal CalcularAjusteEspecial(int idEmpleado, decimal ingresoBase, DateTime fechaInicio, DateTime fechaFin)
        {
            if (idEmpleado <= 0)
            {
                throw new ArgumentException("Debe indicar un empleado válido.");
            }

            if (ingresoBase < 0)
            {
                throw new ArgumentException("El ingreso base no puede ser negativo.");
            }

            if (fechaInicio.Date > fechaFin.Date)
            {
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin.");
            }

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                DateTime fechaIngreso = ObtenerFechaIngreso(connection, idEmpleado);
                int years = Math.Max(0, (DateTime.Today - fechaIngreso.Date).Days / 365);
                decimal porcentajeAntiguedad = Math.Min(years * 0.005m, 0.03m);

                int faltas;
                int tardanzas;
                ObtenerIndicadoresAsistencia(connection, idEmpleado, fechaInicio, fechaFin, out faltas, out tardanzas);

                decimal bonoAntiguedad = ingresoBase * porcentajeAntiguedad;
                decimal bonoAsistencia = faltas == 0 && tardanzas == 0 ? ingresoBase * 0.01m : 0m;
                decimal descuentoFaltas = ingresoBase * Math.Min(faltas * 0.02m, 0.10m);
                decimal descuentoTardanzas = ingresoBase * Math.Min(tardanzas * 0.005m, 0.03m);

                return Math.Round(bonoAntiguedad + bonoAsistencia - descuentoFaltas - descuentoTardanzas, 2);
            }
        }

        private static DateTime ObtenerFechaIngreso(SQLiteConnection connection, int idEmpleado)
        {
            using (SQLiteCommand command = new SQLiteCommand(
                "SELECT FechaIngreso FROM Empleados WHERE IdEmpleado = @id AND Estado = 'Activo';", connection))
            {
                command.Parameters.AddWithValue("@id", idEmpleado);
                object result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new ArgumentException("No se encontró un empleado activo para calcular el ajuste.");
                }

                return DateTime.Parse(Convert.ToString(result));
            }
        }

        private static void ObtenerIndicadoresAsistencia(SQLiteConnection connection, int idEmpleado, DateTime fechaInicio, DateTime fechaFin, out int faltas, out int tardanzas)
        {
            faltas = 0;
            tardanzas = 0;

            using (SQLiteCommand command = new SQLiteCommand(@"SELECT Estado, COUNT(1) AS Total
                FROM Asistencias
                WHERE IdEmpleado = @id AND Fecha BETWEEN @inicio AND @fin
                GROUP BY Estado;", connection))
            {
                command.Parameters.AddWithValue("@id", idEmpleado);
                command.Parameters.AddWithValue("@inicio", fechaInicio.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@fin", fechaFin.ToString("yyyy-MM-dd"));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string estado = Convert.ToString(reader["Estado"]);
                        int total = Convert.ToInt32(reader["Total"]);
                        if (estado == "Falta")
                        {
                            faltas = total;
                        }
                        else if (estado == "Tardanza")
                        {
                            tardanzas = total;
                        }
                    }
                }
            }
        }
    }
}
