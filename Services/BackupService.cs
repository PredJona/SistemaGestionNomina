using System;

namespace SistemaGestionNomina.Services
{
    /// <summary>
    /// Servicio reservado para crear copias de seguridad de la base de datos.
    /// </summary>
    public class BackupService
    {
        /// <summary>
        /// Crea una copia de seguridad de nomina.db en la ruta indicada.
        /// TODO: Validar ruta, copiar base de datos cerrando conexiones y registrar el backup.
        /// </summary>
        public string CrearBackup(string carpetaDestino)
        {
            throw new NotImplementedException("Funcionalidad pendiente para completar por otro integrante.");
        }
    }
}
