using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    public class CreateRestaurantUseCase(
        IRestaurantRepository restaurantRepository,
        IUnitOfWork uow
        ) : ICreateRestaurantUseCase
    {
        public async Task<CreateRestaurantResponse> Execute(CreateRestaurantRequest request)
        {
            var restaurantToCreate = new Domain.Entities.Restaurant
            {
                Name = request.Name,
            };

            var restaurant = await restaurantRepository.AddAsync(restaurantToCreate);
            await uow.CommitAsync();

            return new CreateRestaurantResponse
            {
                RestaurantId = restaurant.Id,
            };
        }
    }
}
