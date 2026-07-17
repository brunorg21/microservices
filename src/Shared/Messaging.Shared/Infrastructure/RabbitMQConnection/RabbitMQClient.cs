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
        private readonly SemaphoreSlim connectionLock = new(1, 1);

        public async Task<IConnection> GetConnection()
        {
            if (connection is { IsOpen: true })
                return connection;

            await connectionLock.WaitAsync();
            try
            {
                if (connection is { IsOpen: true })
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
                logger.LogError(ex, "Failed to connect to RabbitMQ");
                throw new InvalidOperationException(
                    "Failed to connect to RabbitMQ",
                    ex
                );
            }
            finally
            {
                connectionLock.Release();
            }
        }
        public void Dispose()
        {
            connection?.Dispose();
            connectionLock.Dispose();
        }
    }
}
