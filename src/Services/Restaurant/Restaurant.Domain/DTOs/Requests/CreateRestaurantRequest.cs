namespace Restaurant.Domain.DTOs.Requests
{
    public class CreateRestaurantRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<CreateRestaurantTableRequest> Tables { get; set; } = new List<CreateRestaurantTableRequest>();
    }
}
