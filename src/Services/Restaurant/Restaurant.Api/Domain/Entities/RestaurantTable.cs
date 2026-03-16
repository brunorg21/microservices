namespace Restaurant.Api.Domain.Entities
{
    public class RestaurantTable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        // Relations
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}
