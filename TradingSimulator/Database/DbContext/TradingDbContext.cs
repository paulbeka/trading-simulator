using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DbContext
{
    public sealed class TradingDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Trade> Trades => Set<Trade>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Account> Accounts => Set<Account>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
