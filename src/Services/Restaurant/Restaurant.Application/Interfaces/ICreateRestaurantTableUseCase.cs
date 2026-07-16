using Restaurant.Domain.DTOs.Requests;

namespace Restaurant.Application.Interfaces
{
    public interface ICreateRestaurantTableUseCase
    {
        Task Execute(CreateRestaurantTableListRequest request);
    }
}
