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
    public class SparplaeneController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SparplaeneController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/sparplaene
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SparplanCreateDto dto)
        {
            // Wichtig: Sparplan muss einem Depot zugeordnet werden
            var depot = await _context.Depots
                .Include(d => d.Sparplaene)
                .FirstOrDefaultAsync(d => d.Id == dto.DepotId);

            if (depot == null)
                return NotFound($"Depot mit Id {dto.DepotId} nicht gefunden.");

            var sparplan = new SparplanClass(dto.Metal, dto.MonthlyRate, depot);

            depot.AddSparplan(sparplan);
            _context.Sparplaene.Add(sparplan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = sparplan.Id }, sparplan.ToDto());
        }

        // GET: api/sparplaene
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SparplanDto>>> GetAll()
        {
            var sparplaene = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .ToListAsync();

            return Ok(sparplaene.Select(sp => sp.ToDto()));
        }

        // GET: api/sparplaene/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SparplanDto>> GetById(Guid id)
        {
            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sparplan == null)
                return NotFound();

            return Ok(sparplan.ToDto());
        }
    }
}
