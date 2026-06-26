using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    internal class CreateRestaurantUseCase(
        IRestaurantRepository restaurantRepository
        ) : ICreateRestaurantUseCase
    {
        public async Task<CreateRestaurantResponse> Execute(CreateRestaurantRequest request)
        {
            var restaurantToCreate = new Domain.Entities.Restaurant
            {
                Name = request.Name,
            };

            var restaurant = await restaurantRepository.AddAsync(restaurantToCreate);

            return new CreateRestaurantResponse
            {
                RestaurantId = restaurant.Id,
            };
        }
    }
}
