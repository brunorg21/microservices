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

        private const int MaxAttempts = 3;

        public async Task Consume<T>(Func<T, Task> handle) where T : IRoutedMessage
        {
            var route = T.Route;
            var deadLetterExchange = $"{route.Exchange}.dlx";
            var deadLetterQueue = $"{route.Queue}.dlq";

            _connection = await client.GetConnection();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(deadLetterExchange, ExchangeType.Fanout, durable: true);
            await _channel.QueueDeclareAsync(deadLetterQueue, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(deadLetterQueue, deadLetterExchange, routingKey: string.Empty);

            await _channel.ExchangeDeclareAsync(route.Exchange, ExchangeType.Topic, durable: true);
            await _channel.QueueDeclareAsync(
                route.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] = deadLetterExchange
                });
            await _channel.QueueBindAsync(route.Queue, route.Exchange, route.RoutingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                T? data;
                try
                {
                    data = JsonSerializer.Deserialize<T>(message);
                }
                catch (JsonException ex)
                {
                    logger.LogError(ex, "Malformed message from queue {Queue}, sending to DLQ: {Message}", route.Queue, message);
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                if (data is null)
                {
                    logger.LogError("Empty message from queue {Queue}, sending to DLQ", route.Queue);
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                for (var attempt = 1; attempt <= MaxAttempts; attempt++)
                {
                    try
                    {
                        logger.LogInformation("Received message from queue {Queue}: {Message}", route.Queue, message);
                        await handle(data);
                        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                        break;
                    }
                    catch (Exception ex) when (attempt < MaxAttempts)
                    {
                        logger.LogWarning(ex, "Attempt {Attempt}/{MaxAttempts} failed for queue {Queue}", attempt, MaxAttempts, route.Queue);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to handle message from queue {Queue} after {MaxAttempts} attempts, sending to DLQ", route.Queue, MaxAttempts);
                        await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
                    }
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
