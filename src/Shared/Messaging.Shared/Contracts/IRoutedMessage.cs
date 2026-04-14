namespace Messaging.Shared.Contracts
{
    public interface IRoutedMessage
    {
        static abstract MessageRoute Route { get; }
    }
}
