using StackExchange.Redis;

namespace TradingApi.Redis
{
    public static class RedisConnection
    {
        public static ConnectionMultiplexer Connect(string connectionString)
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}
