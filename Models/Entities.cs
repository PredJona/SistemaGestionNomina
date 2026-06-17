using System;
using System.Collections.Generic;

namespace SistemaGestionNomina.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
    }

    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Cargo { get; set; }
        public int IdDepartamento { get; set; }
        public string DepartamentoNombre { get; set; }
        public decimal SalarioBase { get; set; }
        public string Estado { get; set; }
        public DateTime FechaIngreso { get; set; }

        public string NombreCompleto
        {
            get { return (Nombre + " " + Apellido).Trim(); }
        }

        public override string ToString()
        {
            return Codigo + " - " + NombreCompleto;
        }
    }

    public class Asistencia
    {
        public int IdAsistencia { get; set; }
        public int IdEmpleado { get; set; }
        public string EmpleadoNombre { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public string Estado { get; set; }
    }

    public class PeriodoNomina
    {
        public int IdPeriodo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
    }

    public class Nomina
    {
        public int IdNomina { get; set; }
        public int IdPeriodo { get; set; }
        public string PeriodoNombre { get; set; }
        public DateTime FechaCalculo { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal TotalNeto { get; set; }
        public string Estado { get; set; }
        public List<NominaDetalle> Detalles { get; set; }

        public Nomina()
        {
            Detalles = new List<NominaDetalle>();
        }
    }

    public class NominaDetalle
    {
        public int IdDetalle { get; set; }
        public int IdNomina { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string EmpleadoNombre { get; set; }
        public string Departamento { get; set; }
        public decimal SueldoBase { get; set; }
        public decimal Bonos { get; set; }
        public decimal HorasExtra { get; set; }
        public decimal MontoHorasExtra { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal NetoPagar { get; set; }
    }

    public class Comprobante
    {
        public int IdComprobante { get; set; }
        public int IdNomina { get; set; }
        public int IdEmpleado { get; set; }
        public string NumeroComprobante { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string RutaPdf { get; set; }
        public string EmpleadoNombre { get; set; }
        public string CodigoEmpleado { get; set; }
        public string PeriodoNombre { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal NetoPagar { get; set; }
    }

    public class ConfiguracionNomina
    {
        public int IdConfiguracion { get; set; }
        public string NombreParametro { get; set; }
        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
    }

    public class ReporteGenerado
    {
        public int IdReporte { get; set; }
        public string NombreReporte { get; set; }
        public string Tipo { get; set; }
        public string GeneradoPor { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string RutaArchivo { get; set; }
    }
}
