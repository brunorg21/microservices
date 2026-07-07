using Auth.Api.Domain.Security.Token;
using Auth.Api.Infra.Security.Token;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);

            services.AddScoped<ITokenGenerator>(_ => new JwtTokenGenerator(configuration["Jwt:SecretKey"]!));
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Database.AuthDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("Connection")));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<Domain.Repositories.IUserRepository, Repositories.UserRepository>();
        }
    }
}
