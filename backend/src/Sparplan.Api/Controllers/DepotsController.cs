using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sparplan.Domain.Entities;
using Sparplan.Infrastructure.Persistence;
using Sparplan.Api.DTOs.Requests;
using Sparplan.Api.DTOs.Responses;
using Sparplan.Api.DTOs;

namespace Sparplan.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepotsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepotsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/depots  → Erstellt ein Depot
        [HttpPost]
        public async Task<ActionResult<DepotDto>> Create()
        {
            var depot = new Depot();
            _context.Depots.Add(depot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = depot.Id }, depot.ToDto());
        }

        // POST: api/depots/{id}/add-sparplan
        [HttpPost("{id}/add-sparplan")]
        public async Task<IActionResult> AddSparplan(Guid id, [FromBody] SparplanCreateDto dto)
        {
            var depot = await _context.Depots
                .Include(d => d.Sparplaene)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (depot == null)
                return NotFound();

            var sparplan = new SparplanClass(dto.Metal, dto.MonthlyRate, depot);
            depot.AddSparplan(sparplan);
            _context.Sparplaene.Add(sparplan);

            await _context.SaveChangesAsync();

            return Ok(depot.ToDto());
        }

        // GET: api/depots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepotDto>>> GetAll()
        {
            var depots = await _context.Depots
                .Include(d => d.Sparplaene)
                .ThenInclude(sp => sp.Transactions)
                .ToListAsync();

            return Ok(depots.Select(d => d.ToDto()));
        }

        // GET: api/depots/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DepotDto>> GetById(Guid id)
        {
            var depot = await _context.Depots
                .Include(d => d.Sparplaene)
                .ThenInclude(sp => sp.Transactions)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (depot == null)
                return NotFound();

            return Ok(depot.ToDto());
        }
    }
}
