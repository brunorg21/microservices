using Restaurant.Domain.Enums;

namespace Restaurant.Domain.Entities
{
    public class RestaurantQueueEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RestaurantId { get; set; }
        public int Position { get; set; }
        public RestaurantQueueEntriesStatus Status { get; set; } = RestaurantQueueEntriesStatus.WAITING;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? SeatedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
    }
}
