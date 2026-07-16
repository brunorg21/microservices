using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs;
using System.Security.Claims;

namespace Restaurant.Api.User
{
    public class LoggedUser(
        IHttpContextAccessor httpContextAccessor
        ) : ILoggedUser
    {
        public LoggedUserDTO GetLoggedUser()
        {
            var userId = httpContextAccessor!.HttpContext!.User.FindFirst(ClaimTypes.Sid)!.Value;

            var restaurantId = httpContextAccessor!.HttpContext!.User.FindFirst("restaurant").Value;

            return new LoggedUserDTO
            {
                UserId = Guid.Parse(userId),
                RestaurantId = restaurantId != null ? Guid.Parse(restaurantId) : null,
            };
        }
    }
}
