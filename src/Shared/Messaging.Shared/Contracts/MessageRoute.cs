namespace Messaging.Shared.Contracts
{
    public record MessageRoute(string Exchange, string RoutingKey, string Queue);
}
