namespace Messaging.Shared.Settings
{
    internal class RabbitMQSettings
    {
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set;  } = string.Empty;
        public string Password { get; set;  } = string.Empty;
    }
}
