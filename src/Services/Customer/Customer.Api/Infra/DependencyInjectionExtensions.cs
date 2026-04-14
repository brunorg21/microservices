using Auth.Api.Domain.Cache;
using Auth.Api.Infra.Cache;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddCache(services, configuration);
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Database.CustomerDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("Connection")));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<Domain.Repositories.ICustomerRepository, Repositories.CustomerRepository>();
        }

        public static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Redis") ?? string.Empty;

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = "customer:";
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    AbortOnConnectFail = true,
                    EndPoints = { connectionString },
                };
            });

            services.AddScoped<ICacheRepository, RedisCacheRepository>();
        }
    }
}
