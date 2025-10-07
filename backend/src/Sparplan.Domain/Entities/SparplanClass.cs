namespace Sparplan.Domain.Entities
{
    /// <summary>
    /// Repräsentiert einen Edelmetall-Sparplan.
    /// </summary>
    public class SparplanClass
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // FK + Navigation
        public Guid DepotId { get; private set; }
        public Depot Depot { get; private set; }

        public MetalType Metal { get; private set; }
        public decimal MonthlyRate { get; private set; }
        public DateTime StartDate { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; private set; } = true;
        public decimal BalanceInBars { get; private set; }

        /// <summary>
        /// Navigation zu allen Transaktionen dieses Sparplans.
        /// </summary>
        public List<Transaction> Transactions { get; private set; } = new();

        /// <summary>
        /// EF Core benötigt einen parameterlosen Konstruktor.
        /// </summary>
        protected SparplanClass() { }

        /// <summary>
        /// Erstellt einen neuen Sparplan.
        /// </summary>
        public SparplanClass(MetalType metal, decimal monthlyRate, Depot depot)
        {
            if (monthlyRate < 0)
                throw new ArgumentException("Monthly rate cannot be negative.");

            Metal = metal;
            MonthlyRate = monthlyRate;
            Depot = depot ?? throw new ArgumentNullException(nameof(depot));
            DepotId = depot.Id;
        }

        // Zusätzlicher Konstruktor für simulation
        public SparplanClass(MetalType metal, decimal monthlyRate)
        {
            Metal = metal;
            MonthlyRate = monthlyRate;
            IsActive = true;
            BalanceInBars = 0;
        }


        public Transaction AddContribution(decimal amountInBars, decimal amountInCurrency)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot deposit to a closed Sparplan.");
            if (amountInBars <= 0 || amountInCurrency <= 0)
                throw new ArgumentException("Deposit must be positive.");

            var tx = new Transaction(TransactionType.Deposit, amountInCurrency, this);
            Transactions.Add(tx);  // EF erkennt "neu"
            BalanceInBars += amountInBars;
            return tx;
        }

        public Transaction DeductFee(decimal spotPrice)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot apply fees to a closed Sparplan.");
            if (spotPrice <= 0)
                throw new ArgumentException("Spot price must be positive.");

            var now = DateTime.UtcNow;
            var currentQuarterStart = GetQuarterStart(now);

            var lastFee = Transactions
                .Where(t => t.Type == TransactionType.Fee)
                .OrderByDescending(t => t.Date)
                .FirstOrDefault();

            if (lastFee != null && lastFee.Date >= currentQuarterStart)
                throw new InvalidOperationException("Fee already deducted for this quarter.");

            // 1. Gesamtwert in EUR
            var valueInCurrency = BalanceInBars * spotPrice;

            // 2. Quartalsgebühr (0,5 %)
            var feeInCurrency = Math.Round(valueInCurrency * 0.005m, 2);

            // 3. Umrechnung in Bars (5 Nachkommastellen)
            var feeInBars = Math.Round(feeInCurrency / spotPrice, 5);

            if (BalanceInBars < feeInBars)
                throw new InvalidOperationException("Insufficient balance for fee.");

            // 4. Transaktion erzeugen
            var tx = new Transaction(TransactionType.Fee, feeInCurrency, this);
            Transactions.Add(tx);

            // 5. Balance anpassen
            BalanceInBars -= feeInBars;

            return tx;
        }

        public Transaction Close(decimal payoutInCurrency)
        {
            if (!IsActive)
                throw new InvalidOperationException("Sparplan is already closed.");
            if (BalanceInBars <= 0)
                throw new InvalidOperationException("No balance available for payout.");

            IsActive = false;

            var tx = new Transaction(TransactionType.Payout, payoutInCurrency, this);
            Transactions.Add(tx);
            BalanceInBars = 0; // Alles ausgezahlt

            return tx;
        }

        public IEnumerable<SimulationResult> Simulate(
            DateTime from,
            DateTime to,
            decimal monthlyRate,
            IReadOnlyDictionary<DateTime, decimal> prices)
        {
            decimal bars = 0;
            decimal deposits = 0;
            var results = new List<SimulationResult>();

            // wir gruppieren nach Monaten → Monatsletzter Preis
            var monthlyPrices = prices
                .GroupBy(p => new { p.Key.Year, p.Key.Month })
                .Select(g => g.OrderByDescending(x => x.Key).First()) // letzter Tag des Monats
                .OrderBy(x => x.Key);

            foreach (var kvp in monthlyPrices)
            {
                var date = kvp.Key;
                var price = kvp.Value;

                // Einzahlung zum Monatsende
                bars += monthlyRate / price;
                deposits += monthlyRate;

                var marketValue = bars * price;
                var profitLoss = marketValue - deposits;

                results.Add(new SimulationResult(date, deposits, marketValue, profitLoss));
            }

            return results;
        }

        //PRIVATE HELPERS
        private DateTime GetQuarterStart(DateTime date)
        {
            int quarterNumber = (date.Month - 1) / 3; // 0=Q1, 1=Q2, 2=Q3, 3=Q4
            int startMonth = quarterNumber * 3 + 1;
            return new DateTime(date.Year, startMonth, 1);
        }


    }
}
