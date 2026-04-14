using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;
using Restaurant.Infra.Repositories;

namespace Restaurant.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Connection") ?? string.Empty;

            services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            AddRepositories(services);
        }

        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        }
    }
}
