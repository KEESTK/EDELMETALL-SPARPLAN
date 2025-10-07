using Sparplan.Domain.Entities;

namespace Sparplan.Application.Services
{
    public interface IMetalPriceHistoryService
    {
        Task BackfillAsync(MetalType metal, int fromYear, int toYear, CancellationToken ct = default);

        Task<Dictionary<DateTime, decimal>> GetPricesAsync(
            MetalType metal, 
            DateTime from, 
            DateTime to, 
            CancellationToken ct = default);
    }
}
