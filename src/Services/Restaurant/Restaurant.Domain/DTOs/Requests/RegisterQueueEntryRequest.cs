namespace Restaurant.Domain.DTOs.Requests
{
    public class RegisterQueueEntryRequest
    {
        public Guid RestaurantId { get; set;  }
        public Guid CustomerId { get; set;  }
    }
}
