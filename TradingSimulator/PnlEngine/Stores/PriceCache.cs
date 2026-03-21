using System.Collections.Concurrent;

namespace PnlEngine.Stores
{
    internal class PriceCache
    {
        private readonly ConcurrentDictionary<string, decimal> _prices = new();

        public (bool changed, decimal? oldPrice) UpdateIfChanged(string symbol, decimal newPrice)
        {
            decimal? oldPrice = null;
            bool changed = false;

            _prices.AddOrUpdate(
                symbol,
                _ =>
                {
                    changed = true;
                    return newPrice;
                },
                (_, existing) =>
                {
                    oldPrice = existing;

                    if (existing != newPrice)
                    {
                        changed = true;
                        return newPrice;
                    }

                    return existing;
                });

            return (changed, oldPrice);
        }

        public decimal? GetPrice(string ticker)
        {
            bool found = _prices.TryGetValue(ticker, out var price);
            if (found) return price;
            return null;
        }
    }
}
