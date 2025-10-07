namespace Sparplan.Domain.Entities
{
    /// <summary>
    /// Mögliche Arten von Transaktionen innerhalb eines Sparplans.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Einzahlung: erhöht den Bestand des Sparplans.
        /// </summary>
        Deposit,

        /// <summary>
        /// Gebühr: reduziert den Bestand des Sparplans.
        /// </summary>
        Fee,

        /// <summary>
        /// Auszahlung: schließt den Sparplan und verbucht eine Auszahlung.
        /// </summary>
        Payout
    }

    /// <summary>
    /// Repräsentiert eine einzelne Transaktion (Einzahlung, Gebühr oder Auszahlung),
    /// die in einem Sparplan verbucht wird.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Eindeutige Id der Transaktion.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Zeitpunkt der Transaktion (UTC).
        /// </summary>
        public DateTime Date { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Art der Transaktion (Einzahlung, Gebühr, Auszahlung).
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Betrag der Transaktion (fachlich als Bars interpretiert).
        /// Positive Werte erhöhen, negative Werte verringern den Bestand.
        /// </summary>
        public decimal Amount { get; private set; }

        // 🔑 Fremdschlüssel + Navigation
        public Guid SparplanClassId { get; private set; }  
        public SparplanClass Sparplan { get; private set; }

        protected Transaction() { } // EF braucht ctor ohne Parameter

        // Konstruktor für EF / Controller
        public Transaction(TransactionType type, decimal amount, SparplanClass sparplan)
        {
            Id = Guid.NewGuid();
            Type = type;
            Amount = amount;
            Sparplan = sparplan ?? throw new ArgumentNullException(nameof(sparplan));
            SparplanClassId = sparplan.Id;
        }

        // Konstruktor für Domain-Logik (Sparplan erzeugt Transaktion selbst)
        public Transaction(TransactionType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }

    }
}
