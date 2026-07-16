namespace Restaurant.Domain.DTOs
{
    public class LoggedUserDTO
    {
        public Guid UserId { get; set; }
        public Guid? RestaurantId { get; set; }
    }
}
