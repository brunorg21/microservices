using Messaging.Shared.Contracts;
using Messaging.Shared.Infrastructure.RabbitMQConnection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Messaging.Shared.Abstractions.Publishers.RabbitMQ
{
    internal class RabbitMQPublisher(IRabbitMQClient rabbitMQClient) : IRabbitMQPublisher
    {
        public async Task Publish<T>(T data) where T : IRoutedMessage
        {
            var route = T.Route;
            var connection = await rabbitMQClient.GetConnection();

            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(route.Exchange, ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

            await channel.BasicPublishAsync(
                exchange: route.Exchange,
                routingKey: route.RoutingKey,
                body: body
            );
        }
    }
}
