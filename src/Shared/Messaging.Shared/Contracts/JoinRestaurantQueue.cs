namespace Messaging.Shared.Contracts
{
    public record JoinRestaurantQueueEvent : IRoutedMessage
    {
        public static MessageRoute Route => new("customer-exchange", "customer.joined", "customer-joined");

        public Guid AccessToken { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
