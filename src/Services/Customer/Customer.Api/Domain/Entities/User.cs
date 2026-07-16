namespace Auth.Api.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Seats { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid RestaurantId { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
