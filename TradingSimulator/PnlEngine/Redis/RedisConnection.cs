using StackExchange.Redis;

namespace PnlEngine.Redis
{
    public class RedisConnection
    {
        public static ConnectionMultiplexer Connect(string connectionString)
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}