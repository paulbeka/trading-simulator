using StackExchange.Redis;
using System.Globalization;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Redis
{
    public class RedisPriceStore : IPriceStore
    {
        private readonly IDatabase _db;

        public RedisPriceStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<decimal?> GetPrice(string symbol)
        {
            var value = await _db.StringGetAsync($"price:{symbol}");

            if (!value.HasValue) return null;

            return decimal.Parse((string)value!, CultureInfo.InvariantCulture);
        }
    }
}
