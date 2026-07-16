using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    internal class CreateRestaurantTableUseCase
        (
            IRestaurantTableRepository restaurantTableRepository,
            IUnitOfWork uow
        ): ICreateRestaurantTableUseCase
    {
        public async Task Execute(CreateRestaurantTableListRequest request)
        {
            foreach(var table in request.Tables)
            {
                var tableToCreate = new RestaurantTable
                {
                    Name = table.Name,
                    Seats = table.Seats
                };

                await restaurantTableRepository.AddAsync(tableToCreate);
            }

            await uow.CommitAsync();
        }
    }
}
