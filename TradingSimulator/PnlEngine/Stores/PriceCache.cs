using System.Collections.Concurrent;

namespace PnlEngine.Stores
{
    internal class PriceCache
    {
        private readonly ConcurrentDictionary<string, decimal> _prices = new();

        public void Update(string ticker, decimal price)
        {
            _prices[ticker] = price;
        }

        public decimal? GetPrice(string ticker)
        {
            bool found = _prices.TryGetValue(ticker, out var price);
            if (found) return price;
            return null;
        }
    }
}
