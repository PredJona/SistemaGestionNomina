using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using SistemaGestionNomina.Data;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Constructor de reportes avanzados personalizados.
    /// </summary>
    public class AdvancedReportBuilder
    {
        /// <summary>
        /// Construye un reporte personalizado según filtros dinámicos.
        /// </summary>
        public string ConstruirReportePersonalizado(string nombreReporte)
        {
            if (string.IsNullOrWhiteSpace(nombreReporte))
            {
                throw new ArgumentException("Debe indicar el nombre del reporte.");
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# " + nombreReporte.Trim());
            builder.AppendLine();
            builder.AppendLine("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            builder.AppendLine();

            using (SQLiteConnection connection = SQLiteConnectionFactory.CreateConnection())
            {
                AppendMetric(builder, connection, "Empleados activos", "SELECT COUNT(1) FROM Empleados WHERE Estado = 'Activo';");
                AppendMetric(builder, connection, "Departamentos", "SELECT COUNT(1) FROM Departamentos;");
                AppendMetric(builder, connection, "Nominas confirmadas", "SELECT COUNT(1) FROM Nominas WHERE Estado = 'Confirmada';");
                AppendMetric(builder, connection, "Comprobantes generados", "SELECT COUNT(1) FROM Comprobantes;");
                AppendMetric(builder, connection, "Total neto pagado", "SELECT COALESCE(SUM(TotalNeto), 0) FROM Nominas WHERE Estado = 'Confirmada';", true);

                builder.AppendLine();
                builder.AppendLine("## Nómina por departamento");
                builder.AppendLine();
                using (SQLiteCommand command = new SQLiteCommand(@"SELECT d.Nombre AS Departamento,
                    COUNT(DISTINCT e.IdEmpleado) AS Empleados,
                    COALESCE(SUM(nd.NetoPagar), 0) AS Neto
                    FROM Departamentos d
                    LEFT JOIN Empleados e ON e.IdDepartamento = d.IdDepartamento AND e.Estado = 'Activo'
                    LEFT JOIN NominaDetalle nd ON nd.IdEmpleado = e.IdEmpleado
                    GROUP BY d.Nombre
                    ORDER BY d.Nombre;", connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        builder.AppendLine("- " + Convert.ToString(reader["Departamento"]) +
                            ": " + Convert.ToInt32(reader["Empleados"]) + " empleados, B/. " +
                            Convert.ToDecimal(reader["Neto"]).ToString("0.00"));
                    }
                }

                builder.AppendLine();
                builder.AppendLine("## Asistencia reciente");
                builder.AppendLine();
                using (SQLiteCommand command = new SQLiteCommand(@"SELECT Estado, COUNT(1) AS Total
                    FROM Asistencias
                    WHERE Fecha >= date('now', '-30 day')
                    GROUP BY Estado
                    ORDER BY Estado;", connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    bool hasRows = false;
                    while (reader.Read())
                    {
                        hasRows = true;
                        builder.AppendLine("- " + Convert.ToString(reader["Estado"]) + ": " + Convert.ToInt32(reader["Total"]));
                    }

                    if (!hasRows)
                    {
                        builder.AppendLine("- Sin registros de asistencia en los últimos 30 días.");
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Genera un reporte ejecutivo en archivo Markdown para revisión o conversión a PDF.
        /// </summary>
        public string ExportarReportePersonalizado(string nombreReporte)
        {
            string content = ConstruirReportePersonalizado(nombreReporte);
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exports", "PDF");
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, SanitizeFileName(nombreReporte) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".md");
            File.WriteAllText(path, content, Encoding.UTF8);
            return path;
        }

        private static void AppendMetric(StringBuilder builder, SQLiteConnection connection, string label, string sql)
        {
            AppendMetric(builder, connection, label, sql, false);
        }

        private static void AppendMetric(StringBuilder builder, SQLiteConnection connection, string label, string sql, bool currency)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                object value = command.ExecuteScalar();
                if (currency)
                {
                    builder.AppendLine("- " + label + ": B/. " + Convert.ToDecimal(value).ToString("0.00"));
                }
                else
                {
                    builder.AppendLine("- " + label + ": " + Convert.ToInt32(value));
                }
            }
        }

        private static string SanitizeFileName(string value)
        {
            string safe = string.IsNullOrWhiteSpace(value) ? "ReporteAvanzado" : value.Trim();
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safe = safe.Replace(c, '_');
            }

            return safe.Replace(' ', '_');
        }
    }
}
