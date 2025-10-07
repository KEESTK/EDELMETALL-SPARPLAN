using Sparplan.Domain.Entities;

namespace Sparplan.Application.Services
{
    public interface IMetalPriceService
    {
        /// <summary>
        /// Aktueller Spotpreis pro Barren:
        /// - Gold: €/oz
        /// - Silber: €/g * 1000 (also €/Bar)
        /// </summary>
        Task<decimal> GetSpotPricePerBarAsync(MetalType metal, CancellationToken ct = default);

        /// <summary>
        /// Historische Tagespreise für ein Jahr.
        /// Erwartet PricePerUnit:
        /// - Gold: €/oz
        /// - Silber: €/g
        /// Return: Dictionary[Date, PricePerUnit]
        /// </summary>
        Task<IReadOnlyDictionary<DateTime, decimal>> GetHistoricalPricesAsync(
            MetalType metal, int year, CancellationToken ct = default);
    }
}
