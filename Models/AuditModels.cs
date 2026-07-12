using System;

namespace SistemaGestionNomina.Models
{
    public sealed class AuditRecord
    {
        public int IdAuditoria { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }
    }

    public sealed class AuditQuery
    {
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public AuditQuery()
        {
            Limit = 100;
        }
    }
}
