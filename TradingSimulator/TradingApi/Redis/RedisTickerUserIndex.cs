using StackExchange.Redis;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Redis
{
    public class RedisTickerUserIndex : ITickerUserIndex
    {
        private readonly IDatabase _db;

        public RedisTickerUserIndex(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<HashSet<string>> GetUsersFromTicker(string ticker)
        {
            var members = await _db.SetMembersAsync($"ticker:{ticker}:users");
            return members.Select(x => x.ToString()).ToHashSet();
        }

        public async Task AddUserToTicker(string ticker, string user)
        {
            await _db.SetAddAsync($"ticker:{ticker}:users", user);
        }

        public async Task RemoveUserFromTicker(string ticker, string user)
        {
            await _db.SetRemoveAsync($"ticker:{ticker}:users", user);
        }
    }
}
