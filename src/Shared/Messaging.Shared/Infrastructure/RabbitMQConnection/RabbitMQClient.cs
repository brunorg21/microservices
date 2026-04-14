using Messaging.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Messaging.Shared.Infrastructure.RabbitMQConnection
{
    internal class RabbitMQClient(
        IOptions<RabbitMQSettings> settings, 
        ILogger<RabbitMQClient> logger) : IRabbitMQClient, IDisposable
    {
        private IConnection? connection;

        public async Task<IConnection> GetConnection()
        {
            try
            {
                if (connection != null && connection.IsOpen)
                    return connection;

                var factory = new ConnectionFactory
                {
                    HostName = settings.Value.HostName,
                    Port = settings.Value.Port,
                    Password = settings.Value.Password,
                    UserName = settings.Value.UserName
                };

                connection = await factory.CreateConnectionAsync();

                return connection;
            }
            catch (Exception ex) 
            {
                logger.LogError($"Failed to connect to RabbitMQ, {0}", ex);
                throw new InvalidOperationException(
                    "Failed to connect to RabbitMQ",
                    ex
                );
            }
        }
        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
