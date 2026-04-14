using Auth.Api.Application.Interfaces;
using Auth.Api.Application.UseCases;

namespace Auth.Api.Application
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddUseCases(services);
        }

        public static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IJoinRestaurantQueueUseCase, JoinRestaurantQueueUseCase>();
        }
    }
}
