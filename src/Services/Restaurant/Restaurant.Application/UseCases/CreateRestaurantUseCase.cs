using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    public class CreateRestaurantUseCase(
        IRestaurantRepository restaurantRepository,
        IRestaurantTableRepository restaurantTableRepository,
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

            if(request.Tables.Any())
            {
                foreach(var table in request.Tables)
                {
                    var tableToCreate = new RestaurantTable
                    {
                        Name = table.Name,
                        Seats = table.Seats,
                        RestaurantId = restaurant.Id
                    };

                    await restaurantTableRepository.AddAsync(tableToCreate);
                }
            }

            await uow.CommitAsync();

            return new CreateRestaurantResponse
            {
                RestaurantId = restaurant.Id,
            };
        }
    }
}
