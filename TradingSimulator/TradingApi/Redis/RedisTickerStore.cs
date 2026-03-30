using StackExchange.Redis;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Redis
{
    public class RedisTickerStore : ITickerStore
    {
        private readonly IDatabase _db;

        public RedisTickerStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task AddTicker(string ticker)
        {
            await _db.SetAddAsync("tickers:active", ticker);
        }

        public async Task RemoveTicker(string ticker)
        {
            await _db.SetRemoveAsync("tickers:active", ticker);
        }

        public async Task<HashSet<string>> GetAllTickers()
        {
            var values = await _db.SetMembersAsync("tickers:active");
            return values.Select(x => x.ToString()).ToHashSet();
        }
    }
}
