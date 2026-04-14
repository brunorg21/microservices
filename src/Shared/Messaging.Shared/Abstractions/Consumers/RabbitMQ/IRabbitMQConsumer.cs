using Messaging.Shared.Contracts;

namespace Messaging.Shared.Abstractions.Consumers.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        Task Consume<T>(Func<T, Task> handle) where T : IRoutedMessage;
    }
}
