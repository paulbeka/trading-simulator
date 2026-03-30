using MarketDataService.Redis.Interfaces;
using StackExchange.Redis;

namespace MarketDataService.Redis
{
    public class RedisTickerStore : ITickerStore
    {
        private readonly IDatabase _db;

        public RedisTickerStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<IEnumerable<string>> GetAllTickers()
        {
            var values = await _db.SetMembersAsync("tickers:active");
            return values.Select(x => x.ToString()).ToHashSet();
        }
    }
}