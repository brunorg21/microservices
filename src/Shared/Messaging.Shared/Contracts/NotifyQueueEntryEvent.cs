namespace Messaging.Shared.Contracts
{
    public class NotifyQueueEntryEvent : IRoutedMessage
    {
        public static MessageRoute Route => new("notify", "notify.customer", "notify-customer");
        public Guid CustomerId { get; set; }
        public int Position { get; set; }
    }
}