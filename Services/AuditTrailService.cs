using System;
using System.Collections.Generic;
using SistemaGestionNomina.Data;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;

namespace SistemaGestionNomina.Services
{
    public class AuditTrailService
    {
        private readonly AuditRepository repository = new AuditRepository();

        public void RegistrarAccion(string modulo, string accion, string detalle)
        {
            RegistrarCambio(SessionContext.IsAuthenticated ? SessionContext.Username : "sistema",
                modulo, accion, detalle);
        }

        public void RegistrarCambio(string usuario, string modulo, string accion)
        {
            RegistrarCambio(usuario, modulo, accion, string.Empty);
        }

        public void RegistrarCambio(string usuario, string modulo, string accion, string detalle)
        {
            repository.Add(new AuditRecord
            {
                Usuario = Normalize(usuario, "sistema"),
                Modulo = Normalize(modulo, "General"),
                Accion = Normalize(accion, "Acción"),
                Detalle = Normalize(detalle, string.Empty),
                Fecha = DateTime.Now
            });
        }

        public List<AuditRecord> Buscar(AuditQuery query)
        {
            return repository.Search(query ?? new AuditQuery());
        }

        public List<string> ObtenerModulos()
        {
            return repository.GetDistinct("Modulo");
        }

        public List<string> ObtenerAcciones()
        {
            return repository.GetDistinct("Accion");
        }

        public List<string> ObtenerUltimos(int cantidad)
        {
            List<string> items = new List<string>();
            List<AuditRecord> records = repository.Search(new AuditQuery
            {
                Limit = cantidad <= 0 ? 20 : Math.Min(cantidad, 200)
            });
            for (int i = 0; i < records.Count; i++)
            {
                AuditRecord record = records[i];
                items.Add(record.Fecha.ToString("yyyy-MM-dd HH:mm:ss") + " | " + record.Usuario + " | " +
                    record.Modulo + " | " + record.Accion + " | " + record.Detalle);
            }
            return items;
        }

        private static string Normalize(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }
    }
}
