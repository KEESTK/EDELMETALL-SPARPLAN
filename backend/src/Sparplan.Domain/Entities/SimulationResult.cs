namespace Sparplan.Domain.Entities
{
    public class SimulationResult
    {
        public DateTime Date { get; private set; }
        public decimal Deposits { get; private set; }
        public decimal MarketValue { get; private set; }
        public decimal ProfitLoss { get; private set; }

        public SimulationResult(DateTime date, decimal deposits, decimal marketValue, decimal profitLoss)
        {
            Date = date;
            Deposits = deposits;
            MarketValue = marketValue;
            ProfitLoss = profitLoss;
        }
    }
}
