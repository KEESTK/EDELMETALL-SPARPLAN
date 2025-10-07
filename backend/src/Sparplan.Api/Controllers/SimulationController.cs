using Microsoft.AspNetCore.Mvc;
using Sparplan.Application.Services;
using Sparplan.Domain.Entities;

namespace Sparplan.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly IMetalPriceHistoryService _priceHistoryService;

        public SimulationController(IMetalPriceHistoryService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Simulate(
            MetalType metal,
            decimal monthlyRate,
            DateTime from,
            DateTime to,
            CancellationToken ct)
        {
            var prices = await _priceHistoryService.GetPricesAsync(metal, from, to, ct);
            if (prices.Count == 0)
                return NotFound("Keine Preisdaten verfügbar.");

            // Domain nutzen, 
            var sparplan = new SparplanClass(metal, monthlyRate); // depot irrelevant für Simulation, daher angepasste konstruktoraufruff
            var results = sparplan.Simulate(from, to, monthlyRate, prices);

            return Ok(results);
        }
    }

}
