using Messaging.Shared.Abstractions.Consumers.RabbitMQ;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Infrastructure.RabbitMQConnection;
using Messaging.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Shared
{
    public static class DependencyInjectionExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            services.AddSingleton<IRabbitMQClient, RabbitMQClient>();
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
            services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
        }
    }
}
