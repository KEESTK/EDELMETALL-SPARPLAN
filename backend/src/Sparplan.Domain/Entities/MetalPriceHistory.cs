using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sparplan.Domain.Entities
{
    public class MetalPriceHistory
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public MetalType Metal { get; private set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; private set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal PricePerUnit { get; private set; }

        // Konstruktor für EF
        private MetalPriceHistory() { }

        public MetalPriceHistory(MetalType metal, DateTime date, decimal price)
        {
            Metal = metal;
            Date = date.Date; // nur Datum
            PricePerUnit = price;
        }
    }
}