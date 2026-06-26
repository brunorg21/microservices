using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Interfaces;
using Restaurant.Application.UseCases;

namespace Restaurant.Application
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRegisterQueueEntryUseCase, RegisterQueueEntryUseCase>();
            services.AddScoped<ICreateRestaurantUseCase, CreateRestaurantUseCase>();
        }
    }
}
