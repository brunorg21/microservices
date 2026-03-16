using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Domain.Repositories;
using Restaurant.Api.Infra.Database;
using Restaurant.Api.Infra.Repositories;

namespace Restaurant.Api.Infra
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
