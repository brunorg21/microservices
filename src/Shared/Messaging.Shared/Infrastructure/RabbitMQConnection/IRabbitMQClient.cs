using RabbitMQ.Client;

namespace Messaging.Shared.Infrastructure.RabbitMQConnection
{
    internal interface IRabbitMQClient
    {
        Task<IConnection> GetConnection();
    }
}
