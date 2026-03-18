using Customer.Api.DTOs.Request;
using Customer.Api.DTOs.Responses;

namespace Customer.Api.Application.Interfaces
{
    public interface IJoinRestaurantQueueUseCase
    {
        Task<JoinRestaurantQueueResponse> Execute(JoinRestaurantQueueRequest request, CancellationToken ct);
    }
}
