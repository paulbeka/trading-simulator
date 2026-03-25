using PnlEngine.Interfaces;
using StackExchange.Redis;
using System.Globalization;

namespace PnlEngine.Redis
{
    public class RedisPnlStore : IPnlStore
    {
        private readonly IDatabase _db;

        public RedisPnlStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task ChangePnlWithDelta(string user, decimal delta)
        {
            var key = $"user:{user}:pnl";

            var currentValue = await _db.StringGetAsync(key);

            decimal current = currentValue.HasValue
                ? decimal.Parse((string)currentValue!, CultureInfo.InvariantCulture)
                : 0m;

            var updated = current + delta;

            await _db.StringSetAsync(key, updated.ToString(CultureInfo.InvariantCulture));
        }

        public async Task<decimal> Get(string user)
        {
            var value = await _db.StringGetAsync($"user:{user}:pnl");

            return value.HasValue
                ? decimal.Parse((string)value!, CultureInfo.InvariantCulture)
                : 0m;
        }
    }
}