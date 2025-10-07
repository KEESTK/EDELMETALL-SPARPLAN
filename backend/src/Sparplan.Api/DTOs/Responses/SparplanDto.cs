using Sparplan.Domain.Entities;

namespace Sparplan.Api.DTOs.Responses
{
    public class SparplanDto
    {
        public Guid Id { get; set; }
        // direkt Enum, aber durch JsonStringEnumConverter als "Gold"/"Silver" serialisiert
        public MetalType Metal { get; set; }
        public decimal MonthlyRate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public decimal BalanceInBars { get; set; }

        public List<TransactionDto> Transactions { get; set; } = new();
    }
}
