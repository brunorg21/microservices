namespace Auth.Api.DTOs.Request
{
    public class JoinRestaurantQueueRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Seats { get; set; }
    }
}
