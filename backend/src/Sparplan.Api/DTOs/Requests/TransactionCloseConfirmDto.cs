namespace Sparplan.Api.DTOs.Requests
{
   public class TransactionCloseConfirmDto
    {
        public Guid SparplanId { get; set; }
        public string BankAccount { get; set; } = string.Empty;
        public Guid SessionToken { get; set; } // <- muss vom Request kommen
    }
}
