using PnlEngine.Interfaces;
using PnlEngine.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace PnlEngine.Redis
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

            return JsonSerializer.Deserialize<Position>((string)value);
        }

        public async Task Set(string user, string ticker, Position position)
        {
            var json = JsonSerializer.Serialize(position);

            await _db.HashSetAsync($"user:{user}:positions", ticker, json);
        }
    }
}
