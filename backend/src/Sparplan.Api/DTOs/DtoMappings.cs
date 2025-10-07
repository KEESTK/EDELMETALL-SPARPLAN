using Sparplan.Domain.Entities;
using Sparplan.Api.DTOs.Responses;

namespace Sparplan.Api.DTOs
{
    public static class DtoMappings
    {
        public static DepotDto ToDto(this Depot depot) =>
            new DepotDto
            {
                Id = depot.Id,
                Sparplaene = depot.Sparplaene.Select(sp => sp.ToDto()).ToList()
            };

        public static SparplanDto ToDto(this SparplanClass sparplan) =>
            new SparplanDto
            {
                Id = sparplan.Id,
                Metal = sparplan.Metal, // MetalType bleibt Enum (wird in Swagger als String gezeigt dank JsonStringEnumConverter)
                MonthlyRate = sparplan.MonthlyRate,
                StartDate = sparplan.StartDate,
                IsActive = sparplan.IsActive,
                BalanceInBars = sparplan.BalanceInBars,
                Transactions = sparplan.Transactions.Select(t => t.ToDto()).ToList()
            };

        public static TransactionDto ToDto(this Transaction tx) =>
            new TransactionDto
            {
                Id = tx.Id,
                Date = tx.Date,
                Type = tx.Type, // Enum bleibt Enum
                Amount = tx.Amount
            };
    }
}
