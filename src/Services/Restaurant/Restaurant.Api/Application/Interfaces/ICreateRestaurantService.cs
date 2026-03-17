using Restaurant.Api.DTOs;

namespace Restaurant.Api.Application.Interfaces
{
    public interface ICreateRestaurantService
    {
        Task<(int statusCode, object result)> CreateAsync(CreateRestaurantRequest request, CancellationToken ct);
    }
}
