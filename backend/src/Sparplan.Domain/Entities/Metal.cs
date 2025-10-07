namespace Sparplan.Domain.Entities
{
    /// <summary>
    /// Unterstützte Edelmetallarten für Sparpläne.
    /// Definiert die Investitionsbasis (z. B. Gold oder Silber).
    /// </summary>
    public enum MetalType
    {
        /// <summary>
        /// Gold (üblicherweise in Unzen "oz").
        /// </summary>
        Gold,

        /// <summary>
        /// Silber (üblicherweise in Gramm "g").
        /// </summary>
        Silver
    }

    /// <summary>
    /// Fachliches Modell für ein Edelmetall inkl. Einheit.
    /// Wird im Sparplan genutzt, um Metallart und zugehörige Maßeinheit darzustellen.
    /// </summary>
    public class Metal
    {
        /// <summary>
        /// Typ des Edelmetalls (Gold oder Silber).
        /// </summary>
        public MetalType Type { get; private set; }

        /// <summary>
        /// Einheit der Mengenangabe ("oz" für Gold, "g" für Silber).
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// Erstellt ein neues Edelmetall auf Basis des <see cref="MetalType"/>.
        /// </summary>
        /// <param name="type">Art des Metalls (Gold oder Silber).</param>
        public Metal(MetalType type)
        {
            Type = type;
            Unit = type == MetalType.Gold ? "oz" : "g";
        }

        /// <summary>
        /// Liefert eine formatierte Zeichenkette wie "Gold (oz)".
        /// </summary>
        public override string ToString() => $"{Type} ({Unit})";
    }
}
