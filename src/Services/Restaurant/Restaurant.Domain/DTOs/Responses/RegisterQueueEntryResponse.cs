namespace Restaurant.Domain.DTOs.Responses
{
    public class RegisterQueueEntryResponse
    {
        public Guid QueueEntryId { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid CustomerId { get; set; }
        public int Position { get; set; }
    }
}
