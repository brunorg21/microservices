namespace Auth.Api.DTOs.Responses
{
    public class JoinRestaurantQueueResponse
    {
        public Guid CustomerId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
