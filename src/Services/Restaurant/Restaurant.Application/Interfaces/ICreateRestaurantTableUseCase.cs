using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;

namespace Restaurant.Application.Interfaces
{
    internal interface ICreateRestaurantTableUseCase
    {
        Task<CreateRestaurantTableResponse> Execute(List<CreateRestaurantRequest> request);
    }
}
