using Microsoft.EntityFrameworkCore;
using Sparplan.Application.Services;
using Sparplan.Domain.Entities;
using Sparplan.Infrastructure.Persistence;

namespace Sparplan.Infrastructure.Services
{
    public class MetalPriceHistoryService : IMetalPriceHistoryService
    {
        private readonly AppDbContext _db;
        private readonly IMetalPriceService _scraper;

        public MetalPriceHistoryService(AppDbContext db, IMetalPriceService scraper)
        {
            _db = db;
            _scraper = scraper;
        }

        public async Task BackfillAsync(MetalType metal, int fromYear, int toYear, CancellationToken ct = default)
        {
            for (int year = fromYear; year <= toYear; year++)
            {
                var historical = await _scraper.GetHistoricalPricesAsync(metal, year, ct);

                foreach (var kv in historical)
                {
                    var date = kv.Key.Date;
                    var price = kv.Value;

                    bool exists = await _db.MetalPriceHistories
                        .AnyAsync(p => p.Metal == metal && p.Date == date, ct);

                    if (!exists)
                    {
                        _db.MetalPriceHistories.Add(new MetalPriceHistory(metal, date, price));
                    }
                }

                await _db.SaveChangesAsync(ct);
                Console.WriteLine($"[INFO] Stored {historical.Count} {metal} prices for {year}.");
            }
        }

        public async Task<Dictionary<DateTime, decimal>> GetPricesAsync(
            MetalType metal, DateTime from, DateTime to, CancellationToken ct = default)
        {
            return await _db.MetalPriceHistories
                .Where(p => p.Metal == metal && p.Date >= from.Date && p.Date <= to.Date)
                .OrderBy(p => p.Date)
                .ToDictionaryAsync(p => p.Date, p => p.PricePerUnit, ct);
        }
    }
}
