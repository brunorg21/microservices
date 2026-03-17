using Restaurant.Api.Application.Interfaces;
using Restaurant.Api.Application.Services;

namespace Restaurant.Api.Application
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateRestaurantService, CreateRestaurantService>();
        }    
    }
}
