using Sparplan.Domain.Entities;

namespace Sparplan.Api.DTOs.Requests
{
    /// <summary>
    /// DTO für das Anlegen eines neuen Sparplans in einem Depot.
    /// </summary>
    public class SparplanCreateDto
    {

        public Guid DepotId { get; set; }   // Ziel-Depot für den Sparplan
        
        /// <summary>
        /// Edelmetall-Typ (Gold oder Silver)
        /// </summary>
        public MetalType Metal { get; set; }

        /// <summary>
        /// Monatliche Rate in Währungseinheiten
        /// </summary>
        public decimal MonthlyRate { get; set; }
    }
}