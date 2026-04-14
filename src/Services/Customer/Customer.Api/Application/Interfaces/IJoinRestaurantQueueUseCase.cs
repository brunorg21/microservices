using Auth.Api.DTOs.Request;
using Auth.Api.DTOs.Responses;

namespace Auth.Api.Application.Interfaces
{
    public interface IJoinRestaurantQueueUseCase
    {
        Task<JoinRestaurantQueueResponse> Execute(JoinRestaurantQueueRequest request, CancellationToken ct);
    }
}
