using Database.Entities;
using StackExchange.Redis;
using System.Text.Json;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Redis
{
    public class RedisUserPositionStore : IUserPositionStore
    {
        private readonly IDatabase _db;

        public RedisUserPositionStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<Position?> Get(string user, string ticker)
        {
            var value = await _db.HashGetAsync($"user:{user}:positions", ticker);

            if (!value.HasValue) return null;

            return JsonSerializer.Deserialize<Position>(value.ToString());
        }

        public async Task Set(string user, string ticker, Position position)
        {
            var json = JsonSerializer.Serialize(position);

            await _db.HashSetAsync($"user:{user}:positions", ticker, json);
        }

        public async Task Remove(string user, string ticker)
        {
            await _db.HashDeleteAsync($"user:{user}:positions", ticker);
        }
    }
}
