namespace Auth.Api.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Seats { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
