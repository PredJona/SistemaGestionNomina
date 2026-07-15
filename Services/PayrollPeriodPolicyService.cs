using System;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;

namespace SistemaGestionNomina.Services
{
    public class PayrollPeriodPolicyService
    {
        private readonly NominaRepository nominaRepository = new NominaRepository();

        public bool EstaAbierto(DateTime fecha)
        {
            return nominaRepository.GetOverlappingProtectedPeriod(fecha.Date, fecha.Date) == null;
        }

        public bool EstaAbierto(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio.Date > fechaFin.Date) return false;
            return nominaRepository.GetOverlappingProtectedPeriod(fechaInicio.Date, fechaFin.Date) == null;
        }

        public void VerificarPeriodoAbierto(int idPeriodo)
        {
            PeriodoNomina periodo = nominaRepository.GetPeriodoById(idPeriodo);
            if (periodo == null)
                throw new InvalidOperationException("El período no existe.");
            if (periodo.Cerrado)
                throw new InvalidOperationException("El período ya está cerrado. No se pueden realizar cambios.");
        }

        public void VerificarFechasAbiertas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio.Date > fechaFin.Date)
                throw new ArgumentException("La fecha inicial no puede superar la fecha final.");
            if (!EstaAbierto(fechaInicio, fechaFin))
                throw new InvalidOperationException("El rango seleccionado se solapa con un período cerrado o confirmado.");
        }

        public void VerificarCambioLaboralPermitido(DateTime fechaEfectiva)
        {
            if (nominaRepository.HasClosedPeriodAffectedByEmployeeChange(fechaEfectiva.Date))
                throw new InvalidOperationException("La fecha efectiva del cambio afectaría una nómina cerrada.");
        }
    }
}
