using Sparplan.Domain.Entities;

namespace Sparplan.Api.DTOs.Responses
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        // Enum → wird als "Deposit" / "Fee" / "Payout" angezeigt
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
