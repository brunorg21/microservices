using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;

namespace Restaurant.Application.Interfaces
{
    public interface ICreateRestaurantUseCase
    {
        public Task<CreateRestaurantResponse> Execute(CreateRestaurantRequest request);
    }
}
