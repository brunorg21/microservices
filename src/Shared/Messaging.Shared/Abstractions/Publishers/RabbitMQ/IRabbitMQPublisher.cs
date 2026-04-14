using Messaging.Shared.Contracts;

namespace Messaging.Shared.Abstractions.Publishers.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        Task Publish<T>(T data) where T : IRoutedMessage;
    }
}
