namespace Messaging.Shared.Contracts
{
    public record JoinRestaurantQueueEvent : IRoutedMessage
    {
        public static MessageRoute Route => new("customer-exchange", "customer.joined", "customer-joined");

        public Guid AccessToken { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RestaurantId { get; set; }
    }
}
