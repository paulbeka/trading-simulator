using PnlEngine.Interfaces;
using StackExchange.Redis;

namespace PnlEngine.Redis
{
    public class RedisPriceStore : IPriceStore
    {
        private readonly IDatabase _db;

        public RedisPriceStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<(bool changed, decimal? oldPrice)> UpdateIfChanged(string symbol, decimal newPrice)
        {
            var key = $"price:{symbol}";

            var oldValue = await _db.StringGetAsync(key);

            decimal? oldPrice = oldValue.HasValue
                ? decimal.Parse(oldValue!)
                : null;

            if (oldPrice == newPrice)
                return (false, oldPrice);

            await _db.StringSetAsync(key, newPrice.ToString());

            return (true, oldPrice);
        }

        public async Task<decimal?> GetPrice(string symbol)
        {
            var value = await _db.StringGetAsync($"price:{symbol}");
            return value.HasValue ? (decimal)value : null;
        }
    }
}
