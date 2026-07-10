namespace Restaurant.Domain.DTOs.Requests
{
    public class CreateRestaurantTableRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }
    }
}
