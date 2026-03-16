namespace Restaurant.Api.Domain.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relations
        public ICollection<RestaurantTable> RestaurantTables { get; set; } = new List<RestaurantTable>();
    }
}
