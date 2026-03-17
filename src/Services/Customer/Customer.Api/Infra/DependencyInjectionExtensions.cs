using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Database.CustomerDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<Domain.Repositories.ICustomerRepository, Repositories.CustomerRepository>();
        }
    }
}
