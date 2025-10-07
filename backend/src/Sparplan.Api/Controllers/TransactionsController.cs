using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sparplan.Domain.Entities;
using Sparplan.Infrastructure.Persistence;
using Sparplan.Api.DTOs.Requests;
using Sparplan.Api.DTOs.Responses;
using Sparplan.Api.DTOs;
using Sparplan.Application.Services;
using Sparplan.Api.Models;


namespace Sparplan.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMetalPriceService _priceService;

        // temporärer Speicher für Close-Requests
        private static readonly Dictionary<Guid, PendingClosure> _pendingClosures = new();

        public TransactionsController(AppDbContext context, IMetalPriceService priceService)
        {
            _context = context;
            _priceService = priceService;
        }

        // POST: api/transactions/deposit
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionDepositDto dto)
        {
            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == dto.SparplanId);

            if (sparplan == null)
                return NotFound();

            // 1. Preis vom Service holen
            var pricePerBar = await _priceService.GetSpotPricePerBarAsync(sparplan.Metal);
            if (pricePerBar <= 0)
                return StatusCode(500, "Preis konnte nicht ermittelt werden.");

            // 2. Umrechnung: Currency → Bars (5 Nachkommastellen)
            var amountInBars = Math.Round(dto.AmountInCurrency / pricePerBar, 5);

            // 3. Sparplan buchen
            var tx = sparplan.AddContribution(amountInBars, dto.AmountInCurrency);

            _context.Transactions.Add(tx); // explizit tracken

            await _context.SaveChangesAsync();
            return Ok(sparplan.ToDto());
        }

        // POST: api/transactions/fee
        [HttpPost("fee")]
        public async Task<IActionResult> Fee([FromBody] TransactionFeeDto dto)
        {
            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == dto.SparplanId);

            if (sparplan == null)
                return NotFound($"Sparplan {dto.SparplanId} nicht gefunden.");

            // Spotpreis für das Metall abrufen
            var spotPrice = await _priceService.GetSpotPricePerBarAsync(sparplan.Metal);
            if (spotPrice <= 0)
                return StatusCode(500, "Konnte Spotpreis nicht ermitteln.");

            try
            {
                // Quartalsgebühr buchen
                var tx = sparplan.DeductFee(spotPrice);

                _context.Transactions.Add(tx);
                await _context.SaveChangesAsync();

                return Ok(sparplan.ToDto());
            }
            catch (InvalidOperationException ex)
            {
                // z. B. wenn Gebühr schon im Quartal gebucht wurde oder Balance zu klein ist
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unerwarteter Fehler: {ex.Message}");
            }
        }


        //Request Close (generiert SessionToken)
        [HttpPost("close/request")]
        public async Task<IActionResult> RequestClose([FromBody] TransactionCloseRequestDto dto)
        {
            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == dto.SparplanId);

            if (sparplan == null)
                return NotFound();

            var pricePerUnit = await _priceService.GetSpotPricePerBarAsync(sparplan.Metal);
            var payoutAmount = Math.Round(sparplan.BalanceInBars * pricePerUnit, 2);

            var token = Guid.NewGuid();
            _pendingClosures[token] = new PendingClosure
            {
                SessionToken = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            return Ok(new
            {
                SessionToken = token,
                Metal = sparplan.Metal.ToString(),
                Bars = sparplan.BalanceInBars,
                EstimatedPayout = payoutAmount,
                Message = "Bitte bestätigen Sie mit Ihrer Bankverbindung innerhalb von 5 Minuten."
            });
        }

        //Confirm Close (mit BankAccount + gültigem Token)
        [HttpPost("close/confirm")]
        public async Task<IActionResult> ConfirmClose([FromBody] TransactionCloseConfirmDto dto)
        {
            if (!_pendingClosures.TryGetValue(dto.SessionToken, out var pending) ||
                pending.ExpiresAt < DateTime.UtcNow)
            {
                return BadRequest("Ungültiger oder abgelaufener Token.");
            }

            _pendingClosures.Remove(dto.SessionToken);

            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == dto.SparplanId);

            if (sparplan == null)
                return NotFound();

            var pricePerUnit = await _priceService.GetSpotPricePerBarAsync(sparplan.Metal);
            var payoutAmount = Math.Round(sparplan.BalanceInBars * pricePerUnit, 2);

            var tx = sparplan.Close(payoutAmount);

            _context.Transactions.Add(tx);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Sparplan wurde geschlossen.",
                PaidOut = payoutAmount,
                BankAccount = dto.BankAccount
            });
        }

        // GET: api/transactions/{sparplanId}
        [HttpGet("{sparplanId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(Guid sparplanId)
        {
            var sparplan = await _context.Sparplaene
                .Include(sp => sp.Transactions)
                .FirstOrDefaultAsync(sp => sp.Id == sparplanId);

            if (sparplan == null)
                return NotFound();

            return Ok(sparplan.Transactions.Select(t => t.ToDto()));
        }
    }
}
