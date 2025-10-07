using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Sparplan.Application.Services;
using Sparplan.Domain.Entities;

namespace Sparplan.Infrastructure.Background
{
    public class PriceFetchWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<PriceFetchWorker> _logger;

        public PriceFetchWorker(IServiceProvider provider, ILogger<PriceFetchWorker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // jeden Tag einmal laufen
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var historyService = scope.ServiceProvider.GetRequiredService<IMetalPriceHistoryService>();

                try
                {
                    _logger.LogInformation("Fetching daily metal prices...");

                    // Gold + Silber speichern (heutiger Tag)
                    await historyService.BackfillAsync(MetalType.Gold, DateTime.UtcNow.Year, DateTime.UtcNow.Year, stoppingToken);
                    await historyService.BackfillAsync(MetalType.Silver, DateTime.UtcNow.Year, DateTime.UtcNow.Year, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while fetching daily metal prices.");
                }

                // 24h warten
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
