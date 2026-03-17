using Restaurant.Api.Application.Interfaces;
using Restaurant.Api.Domain.Repositories;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Application.Services
{
    public class CreateRestaurantService(
        IRestaurantRepository restaurantRepository
        ) : ICreateRestaurantService
    {
        public async Task<(int statusCode, object result)> CreateAsync(CreateRestaurantRequest request, CancellationToken ct)
        {
            try
            {
                var restaurant = new Domain.Entities.Restaurant
                {
                    Name = request.Name
                };

                var result = await restaurantRepository.AddAsync(restaurant, ct);

                return (201, result);
            } catch(Exception ex)
            {
                return (500, new
                {
                    Message = "An error occurred while creating the restaurant.",
                    Details = ex.Message
                });
            }
        }
    }
}
