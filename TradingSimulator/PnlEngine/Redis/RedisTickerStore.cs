using PnlEngine.Interfaces;
using StackExchange.Redis;

namespace PnlEngine.Redis
{
    public class RedisTickerStore : ITickerStore
    {
        private readonly IDatabase _db;

        public RedisTickerStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public Task AddTicker(string ticker)
        {
            return _db.SetAddAsync("tickers", ticker);
        }
    }
}
