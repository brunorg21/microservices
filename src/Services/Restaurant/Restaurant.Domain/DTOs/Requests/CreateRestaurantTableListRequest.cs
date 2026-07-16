namespace Restaurant.Domain.DTOs.Requests
{
    public class CreateRestaurantTableListRequest
    {
        public List<CreateRestaurantTableRequest> Tables { get; set; } = new List<CreateRestaurantTableRequest>();
    }
}
