using Messaging.Shared.Contracts;
using Messaging.Shared.Infrastructure.RabbitMQConnection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Messaging.Shared.Abstractions.Consumers.RabbitMQ
{
    internal class RabbitMQConsumer(IRabbitMQClient client, ILogger<RabbitMQConsumer> logger) : IRabbitMQConsumer, IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;

        public async Task Consume<T>(Func<T, Task> handle) where T : IRoutedMessage
        {
            var route = T.Route;

            _connection = await client.GetConnection();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(route.Exchange, ExchangeType.Topic, durable: true);
            await _channel.QueueDeclareAsync(route.Queue, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(route.Queue, route.Exchange, route.RoutingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<T>(message);

                if (data is null)
                {
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                try
                {
                    logger.LogInformation("Received message from queue {Queue}: {Message}", route.Queue, message);
                    await handle(data);
                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to handle message from queue {Queue}", route.Queue);
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(route.Queue, autoAck: false, consumer: consumer);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.DisposeAsync();

            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}
