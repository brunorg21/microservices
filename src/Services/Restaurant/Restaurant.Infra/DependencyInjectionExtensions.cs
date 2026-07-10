using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;
using Restaurant.Infra.Repositories;
using StackExchange.Redis;

namespace Restaurant.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Connection") ?? string.Empty;
            string redisConnection = configuration.GetConnectionString("Redis") ?? string.Empty;

            services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            // Redis LOCK
            services.AddSingleton<IDistributedLockProvider>(_ =>
            {
                var mutplx = ConnectionMultiplexer.Connect(redisConnection);
                return new RedisDistributedSynchronizationProvider(mutplx.GetDatabase());
            });

            AddRepositories(services);
        }

        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IRestaurantQueueEntryRepository, RestaurantQueueEntryRepository>();
            services.AddScoped<IRestaurantTableRepository, RestaurantTableRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
