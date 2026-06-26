using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;

namespace Restaurant.Application.Interfaces
{
    public interface IRegisterQueueEntryUseCase
    {
        public Task<RegisterQueueEntryResponse> Execute(RegisterQueueEntryRequest request);
    }
}
