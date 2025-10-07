namespace Sparplan.Domain.Entities
{
    /// <summary>
    /// Repräsentiert ein Depot, das mehrere Sparpläne enthalten kann.
    /// </summary>
    public class Depot
    {
        /// <summary>
        /// Eindeutige Id des Depots.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Navigation Property für alle Sparpläne in diesem Depot.
        /// </summary>
        public List<SparplanClass> Sparplaene { get; private set; } = new();

        /// <summary>
        /// Fügt dem Depot einen Sparplan hinzu.
        /// </summary>
        public void AddSparplan(SparplanClass sparplan)
        {
            if (sparplan == null)
                throw new ArgumentNullException(nameof(sparplan));

            Sparplaene.Add(sparplan);
        }

        /// <summary>
        /// Berechnet den Gesamtbestand aller Sparpläne in Bars.
        /// </summary>
        public decimal GetTotalBalanceInBars()
        {
            return Sparplaene.Sum(sp => sp.BalanceInBars);
        }

        /// <summary>
        /// Liefert alle aktiven Sparpläne.
        /// </summary>
        public IEnumerable<SparplanClass> GetActivePlans()
        {
            return Sparplaene.Where(sp => sp.IsActive);
        }
    }
}
