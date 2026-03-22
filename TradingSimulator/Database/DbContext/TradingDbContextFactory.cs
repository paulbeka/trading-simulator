using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.DbContext
{
    public sealed class TradingDbContextFactory : IDesignTimeDbContextFactory<TradingDbContext>
    {
        public TradingDbContext CreateDbContext(string[] args)
        {
            // TODO: maybe don't have the really unsafe fallback.
            var connectionString =
                Environment.GetEnvironmentVariable("TRADING_DB_CONNECTION_STRING")
                ?? "Host=localhost;Port=5432;Database=trading_db;Username=admin;Password=admin";

            var optionsBuilder = new DbContextOptionsBuilder<TradingDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TradingDbContext(optionsBuilder.Options);
        }
    }
}
