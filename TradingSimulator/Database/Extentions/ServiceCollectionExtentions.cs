using Database.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTradingDatabase(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<TradingDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
