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
            PeriodoNomina periodo = nominaRepository.GetPeriodoByFechas(fecha, fecha);
            return periodo == null || !periodo.Cerrado;
        }

        public bool EstaAbierto(DateTime fechaInicio, DateTime fechaFin)
        {
            PeriodoNomina periodo = nominaRepository.GetPeriodoByFechas(fechaInicio, fechaFin);
            return periodo == null || !periodo.Cerrado;
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
            if (!EstaAbierto(fechaInicio, fechaFin))
                throw new InvalidOperationException("El período de fechas seleccionado ya está cerrado.");
        }
    }
}
