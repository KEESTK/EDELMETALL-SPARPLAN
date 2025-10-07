namespace Sparplan.Api.DTOs
{
    public class SimulationResultDto
    {
        public List<SimulationPointDto> Points { get; set; } = new();
    }

    public class SimulationPointDto
    {
        public DateTime Date { get; set; }
        public decimal Deposits { get; set; }     // kumulierte Einzahlungen
        public decimal Bars { get; set; }         // Bestände in Barren
        public decimal MarketValue { get; set; }  // Marktwert in €
        public decimal ProfitLoss { get; set; }   // Gewinn/Verlust
    }
}
