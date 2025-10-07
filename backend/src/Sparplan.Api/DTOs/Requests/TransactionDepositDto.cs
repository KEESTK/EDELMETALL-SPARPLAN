namespace Sparplan.Api.DTOs.Requests
{
    /// <summary>
    /// DTO für eine Einzahlung (Deposit) in einen Sparplan.
    /// </summary>
    public class TransactionDepositDto
    {
        public Guid SparplanId { get; set; }
        public decimal AmountInCurrency { get; set; }
    }
}
