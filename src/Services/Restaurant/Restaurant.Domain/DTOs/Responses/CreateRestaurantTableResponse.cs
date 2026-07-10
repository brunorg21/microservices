namespace Restaurant.Domain.DTOs.Responses
{
    public class CreateRestaurantTableResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
