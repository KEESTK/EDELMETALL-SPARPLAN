using System.Globalization;
using HtmlAgilityPack;
using Sparplan.Application.Services;
using Sparplan.Domain.Entities;

namespace Sparplan.Infrastructure.Services
{
    public class MetalPriceScraper : IMetalPriceService
    {
        private readonly HttpClient _http = new HttpClient();

        // === Live spot (already used by /transactions/deposit) ===================
        public async Task<decimal> GetSpotPricePerBarAsync(MetalType metal, CancellationToken ct = default)
        {
            try
            {
                var url = "https://www.exchange-rates.org/de/edelmetalle";
                var html = await _http.GetStringAsync(url, ct);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var rows = doc.DocumentNode.SelectNodes("//table//tr") ?? Enumerable.Empty<HtmlNode>();

                decimal? goldPerOz = null;
                decimal? silverPerGram = null;

                foreach (var row in rows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols == null || cols.Count < 2) continue;

                    var metalName = Clean(cols[0].InnerText);
                    var priceText = CleanPrice(cols[1].InnerText);
                    if (!TryParseDecimalEur(priceText, out var price)) continue;

                    if (metalName.Contains("Gold", StringComparison.OrdinalIgnoreCase))
                        goldPerOz = price;          // €/oz
                    else if (metalName.Contains("Silber", StringComparison.OrdinalIgnoreCase))
                        silverPerGram = price;      // €/g
                }

                return metal switch
                {
                    MetalType.Gold => goldPerOz ?? 3282.98m,                  // fallback €/oz
                    MetalType.Silver => (silverPerGram ?? 1.2743m) * 1000m,    // €/Bar (1000g)
                    _ => throw new NotSupportedException()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Spotprice scraping failed: {ex.Message}");
                // Fallback dummy values
                return metal switch
                {
                    MetalType.Gold => 3282.98m,   // €/oz
                    MetalType.Silver => 1274.3m,  // €/1000g
                    _ => throw new NotSupportedException()
                };
            }
        }

        // === Historical prices by year (€/oz for Gold, €/g for Silver) ===========
        public async Task<IReadOnlyDictionary<DateTime, decimal>> GetHistoricalPricesAsync(
            MetalType metal, int year, CancellationToken ct = default)
        {
            try
            {
                var url = metal == MetalType.Gold
                    ? $"https://www.exchange-rates.org/de/edelmetalle/gold/deutschland/{year}"
                    : $"https://www.exchange-rates.org/de/edelmetalle/silber/deutschland/{year}";

                var html = await _http.GetStringAsync(url, ct);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Heuristic parser: look for rows with date & price
                // Try common table structures first
                var rows = doc.DocumentNode.SelectNodes("//table[contains(@class,'table')]/tbody/tr")
                           ?? doc.DocumentNode.SelectNodes("//table//tr")
                           ?? new HtmlNodeCollection(null);

                var result = new Dictionary<DateTime, decimal>();

                foreach (var row in rows)
                {
                    var tds = row.SelectNodes("td");
                    if (tds == null || tds.Count < 2) continue;

                    var dateText = Clean(tds[0].InnerText);
                    var priceText = CleanPrice(tds[1].InnerText);

                    if (!TryParseDateDe(dateText, out var date)) continue;
                    if (!TryParseDecimalEur(priceText, out var price)) continue;

                    // We expect:
                    // - Gold: €/oz
                    // - Silver: €/g
                    result[date.Date] = price;
                }

                if (result.Count > 0)
                {
                    Console.WriteLine($"[INFO] Scraped {result.Count} {metal} prices for {year}.");
                    return result;
                }

                Console.WriteLine($"[WARN] No rows parsed for {metal} {year}. Using dummy.");
                return GenerateDummyRange(metal, year);   // statt GenerateDummyYear
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Historical scraping failed for {metal} {year}: {ex.Message}");
                return GenerateDummyRange(metal, year);   // statt nur ein Jahr
            }
        }

        // === Helpers =============================================================
        private static string Clean(string s)
            => (s ?? string.Empty).Replace("\n", " ").Replace("\r", " ").Trim();

        private static string CleanPrice(string s)
        {
            // Examples: "3.282,98 €" or "1,2743 €"
            return (s ?? string.Empty)
                .Replace("€", "", StringComparison.OrdinalIgnoreCase)
                .Replace(" ", "")
                .Replace(".", "")     // thousands sep
                .Replace(",", ".");   // decimal sep
        }

        private static bool TryParseDecimalEur(string s, out decimal value)
            => decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value);

        private static bool TryParseDateDe(string s, out DateTime date)
        {
            // Try several German/ISO patterns just in case
            var formats = new[]
            {
                "dd.MM.yyyy", "d.M.yyyy", "dd.MM.yy", "yyyy-MM-dd", "dd MMM yyyy", "dd. MMM yyyy",
            };
            foreach (var f in formats)
            {
                if (DateTime.TryParseExact(s, f, new CultureInfo("de-DE"), DateTimeStyles.None, out date))
                    return true;
            }
            // Also try a generic parse with de-DE culture
            return DateTime.TryParse(s, new CultureInfo("de-DE"), DateTimeStyles.None, out date);
        }

        private static IReadOnlyDictionary<DateTime, decimal> GenerateDummyYear(MetalType metal, int year)
        {
            // Simple synthetic curve: base + slight monthly drift + tiny daily noise
            decimal basePrice = metal == MetalType.Gold ? 3200m : 1.20m; // €/oz or €/g
            var rnd = new Random(year * (metal == MetalType.Gold ? 1 : 2));

            var dict = new Dictionary<DateTime, decimal>();
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);

            for (var d = start; d <= end; d = d.AddDays(1))
            {
                var monthDrift = (d.Month - 6) * 0.004m;              // -0.024 .. +0.024
                var dailyNoise = (decimal)(rnd.NextDouble() - 0.5) * 0.01m; // +-1%
                var factor = 1m + monthDrift + dailyNoise;

                var price = Math.Round(basePrice * factor, 4, MidpointRounding.AwayFromZero);

                // Gold stays €/oz; Silver stays €/g
                dict[d] = price <= 0 ? basePrice : price;
            }
            Console.WriteLine($"[INFO] Generated dummy prices for {metal} {year}: {dict.Count} days.");
            return dict;
        }

        private static IReadOnlyDictionary<DateTime, decimal> GenerateDummyRange(MetalType metal, int toYear, int yearsBack = 5)
        {
            var dict = new Dictionary<DateTime, decimal>();

            for (int year = toYear - yearsBack + 1; year <= toYear; year++)
            {
                var yearly = GenerateDummyYear(metal, year);
                foreach (var kv in yearly)
                    dict[kv.Key] = kv.Value;
            }

            Console.WriteLine($"[INFO] Generated dummy prices for {metal} {toYear - yearsBack + 1}–{toYear}: {dict.Count} days total.");
            return dict;
        }

    }
}
