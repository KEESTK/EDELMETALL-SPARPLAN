namespace Sparplan.Api.Models
{
    public class PendingClosure
    {
        public Guid SessionToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
