using Customer.Api.Application.Interfaces;
using Customer.Api.Application.UseCases;

namespace Customer.Api.Application
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
