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
        private int idEmpleado;
        private string codigo;
        private string nombre;
        private string apellido;
        private string cedula;
        private string cargo;
        private int idDepartamento;
        private string departamentoNombre;
        private decimal salarioBase;
        private string estado;
        private DateTime fechaIngreso;

        public int IdEmpleado
        {
            get { return idEmpleado; }
            set { idEmpleado = value; }
        }

        public string Codigo
        {
            get { return codigo; }
            set { codigo = EntityValidation.NormalizeRequired(value, "El codigo del empleado"); }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = EntityValidation.NormalizeRequired(value, "El nombre del empleado"); }
        }

        public string Apellido
        {
            get { return apellido; }
            set { apellido = EntityValidation.NormalizeOptional(value); }
        }

        public string Cedula
        {
            get { return cedula; }
            set { cedula = EntityValidation.NormalizeRequired(value, "La cedula del empleado"); }
        }

        public string Cargo
        {
            get { return cargo; }
            set { cargo = EntityValidation.NormalizeRequired(value, "El cargo del empleado"); }
        }

        public int IdDepartamento
        {
            get { return idDepartamento; }
            set { idDepartamento = value; }
        }

        public string DepartamentoNombre
        {
            get { return departamentoNombre; }
            set { departamentoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public decimal SalarioBase
        {
            get { return salarioBase; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El salario base");
                salarioBase = value;
            }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = EntityValidation.NormalizeAllowedRequired(value, "El estado del empleado", "Activo", "Inactivo"); }
        }

        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set
            {
                EntityValidation.RequireValidDate(value, "La fecha de ingreso");
                fechaIngreso = value;
            }
        }

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
        private int idAsistencia;
        private int idEmpleado;
        private string empleadoNombre;
        private DateTime fecha;
        private TimeSpan? horaEntrada;
        private TimeSpan? horaSalida;
        private decimal horasTrabajadas;
        private string estado;

        public int IdAsistencia
        {
            get { return idAsistencia; }
            set { idAsistencia = value; }
        }

        public int IdEmpleado
        {
            get { return idEmpleado; }
            set
            {
                EntityValidation.RequirePositiveId(value, "El empleado");
                idEmpleado = value;
            }
        }

        public string EmpleadoNombre
        {
            get { return empleadoNombre; }
            set { empleadoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set
            {
                EntityValidation.RequireValidDate(value, "La fecha de asistencia");
                fecha = value;
            }
        }

        public TimeSpan? HoraEntrada
        {
            get { return horaEntrada; }
            set { horaEntrada = value; }
        }

        public TimeSpan? HoraSalida
        {
            get { return horaSalida; }
            set { horaSalida = value; }
        }

        public decimal HorasTrabajadas
        {
            get { return horasTrabajadas; }
            set
            {
                EntityValidation.RequireNonNegative(value, "Las horas trabajadas");
                horasTrabajadas = value;
            }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = EntityValidation.NormalizeAllowedRequired(value, "El estado de asistencia", "Puntual", "Tardanza", "Falta", "Permiso"); }
        }
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
        private int idNomina;
        private int idPeriodo;
        private string periodoNombre;
        private DateTime fechaCalculo;
        private decimal totalIngresos;
        private decimal totalDeducciones;
        private decimal totalNeto;
        private string estado;
        private List<NominaDetalle> detalles;

        public Nomina()
        {
            Detalles = new List<NominaDetalle>();
        }

        public int IdNomina
        {
            get { return idNomina; }
            set { idNomina = value; }
        }

        public int IdPeriodo
        {
            get { return idPeriodo; }
            set { idPeriodo = value; }
        }

        public string PeriodoNombre
        {
            get { return periodoNombre; }
            set { periodoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public DateTime FechaCalculo
        {
            get { return fechaCalculo; }
            set
            {
                EntityValidation.RequireValidDate(value, "La fecha de calculo");
                fechaCalculo = value;
            }
        }

        public decimal TotalIngresos
        {
            get { return totalIngresos; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de ingresos");
                totalIngresos = value;
            }
        }

        public decimal TotalDeducciones
        {
            get { return totalDeducciones; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de deducciones");
                totalDeducciones = value;
            }
        }

        public decimal TotalNeto
        {
            get { return totalNeto; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total neto");
                totalNeto = value;
            }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = EntityValidation.NormalizeOptional(value); }
        }

        public List<NominaDetalle> Detalles
        {
            get { return detalles; }
            set { detalles = value ?? new List<NominaDetalle>(); }
        }
    }

    public class NominaDetalle
    {
        private int idDetalle;
        private int idNomina;
        private int idEmpleado;
        private string codigoEmpleado;
        private string empleadoNombre;
        private string departamento;
        private decimal sueldoBase;
        private decimal bonos;
        private decimal horasExtra;
        private decimal montoHorasExtra;
        private decimal totalIngresos;
        private decimal totalDeducciones;
        private decimal netoPagar;

        public int IdDetalle
        {
            get { return idDetalle; }
            set { idDetalle = value; }
        }

        public int IdNomina
        {
            get { return idNomina; }
            set { idNomina = value; }
        }

        public int IdEmpleado
        {
            get { return idEmpleado; }
            set { idEmpleado = value; }
        }

        public string CodigoEmpleado
        {
            get { return codigoEmpleado; }
            set { codigoEmpleado = EntityValidation.NormalizeOptional(value); }
        }

        public string EmpleadoNombre
        {
            get { return empleadoNombre; }
            set { empleadoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public string Departamento
        {
            get { return departamento; }
            set { departamento = EntityValidation.NormalizeOptional(value); }
        }

        public decimal SueldoBase
        {
            get { return sueldoBase; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El sueldo base");
                sueldoBase = value;
            }
        }

        public decimal Bonos
        {
            get { return bonos; }
            set
            {
                EntityValidation.RequireNonNegative(value, "Los bonos");
                bonos = value;
            }
        }

        public decimal HorasExtra
        {
            get { return horasExtra; }
            set
            {
                EntityValidation.RequireNonNegative(value, "Las horas extra");
                horasExtra = value;
            }
        }

        public decimal MontoHorasExtra
        {
            get { return montoHorasExtra; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El monto de horas extra");
                montoHorasExtra = value;
            }
        }

        public decimal TotalIngresos
        {
            get { return totalIngresos; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de ingresos");
                totalIngresos = value;
            }
        }

        public decimal TotalDeducciones
        {
            get { return totalDeducciones; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de deducciones");
                totalDeducciones = value;
            }
        }

        public decimal NetoPagar
        {
            get { return netoPagar; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El neto a pagar");
                netoPagar = value;
            }
        }
    }

    public class Comprobante
    {
        private int idComprobante;
        private int idNomina;
        private int idEmpleado;
        private string numeroComprobante;
        private DateTime fechaGeneracion;
        private string rutaPdf;
        private string empleadoNombre;
        private string codigoEmpleado;
        private string periodoNombre;
        private decimal totalIngresos;
        private decimal totalDeducciones;
        private decimal netoPagar;

        public int IdComprobante
        {
            get { return idComprobante; }
            set { idComprobante = value; }
        }

        public int IdNomina
        {
            get { return idNomina; }
            set { idNomina = value; }
        }

        public int IdEmpleado
        {
            get { return idEmpleado; }
            set { idEmpleado = value; }
        }

        public string NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = EntityValidation.NormalizeRequired(value, "El numero de comprobante"); }
        }

        public DateTime FechaGeneracion
        {
            get { return fechaGeneracion; }
            set
            {
                EntityValidation.RequireValidDate(value, "La fecha de generacion");
                fechaGeneracion = value;
            }
        }

        public string RutaPdf
        {
            get { return rutaPdf; }
            set { rutaPdf = EntityValidation.NormalizeOptional(value); }
        }

        public string EmpleadoNombre
        {
            get { return empleadoNombre; }
            set { empleadoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public string CodigoEmpleado
        {
            get { return codigoEmpleado; }
            set { codigoEmpleado = EntityValidation.NormalizeOptional(value); }
        }

        public string PeriodoNombre
        {
            get { return periodoNombre; }
            set { periodoNombre = EntityValidation.NormalizeOptional(value); }
        }

        public decimal TotalIngresos
        {
            get { return totalIngresos; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de ingresos");
                totalIngresos = value;
            }
        }

        public decimal TotalDeducciones
        {
            get { return totalDeducciones; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El total de deducciones");
                totalDeducciones = value;
            }
        }

        public decimal NetoPagar
        {
            get { return netoPagar; }
            set
            {
                EntityValidation.RequireNonNegative(value, "El neto a pagar");
                netoPagar = value;
            }
        }
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

    internal static class EntityValidation
    {
        public static string NormalizeRequired(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(fieldName + " es obligatorio.");
            }

            return value.Trim();
        }

        public static string NormalizeOptional(string value)
        {
            return value == null ? null : value.Trim();
        }

        public static string NormalizeAllowedRequired(string value, string fieldName, params string[] allowedValues)
        {
            string normalized = NormalizeRequired(value, fieldName);
            for (int i = 0; i < allowedValues.Length; i++)
            {
                if (string.Equals(normalized, allowedValues[i], StringComparison.OrdinalIgnoreCase))
                {
                    return allowedValues[i];
                }
            }

            throw new ArgumentException(fieldName + " debe ser: " + string.Join(", ", allowedValues) + ".");
        }

        public static void RequireNonNegative(decimal value, string fieldName)
        {
            if (value < 0)
            {
                throw new ArgumentException(fieldName + " no puede ser negativo.");
            }
        }

        public static void RequirePositiveId(int value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ArgumentException(fieldName + " debe ser mayor que cero.");
            }
        }

        public static void RequireValidDate(DateTime value, string fieldName)
        {
            if (value == DateTime.MinValue)
            {
                throw new ArgumentException(fieldName + " debe ser una fecha valida.");
            }
        }
    }
}
