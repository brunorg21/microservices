using Customer.Api.Application.Interfaces;
using Customer.Api.Domain.Cache;
using Customer.Api.Domain.Repositories;
using Customer.Api.DTOs.Request;
using Customer.Api.DTOs.Responses;
using Microsoft.Extensions.Caching.Distributed;

namespace Customer.Api.Application.UseCases
{
    public class JoinRestaurantQueueUseCase
        (
            ICustomerRepository customerRepository,
            ICacheRepository cache
        ): IJoinRestaurantQueueUseCase
    {
        public async Task<JoinRestaurantQueueResponse> Execute(JoinRestaurantQueueRequest request, CancellationToken ct)
        {
            Guid sessionId = Guid.NewGuid();

            var customerToCreate = new Domain.Entities.Customer
            {
                AccessToken = sessionId.ToString(),
                Name = request.Name,
                Phone = request.Phone,
                Seats = request.Seats
            };

            var customer = await customerRepository.AddAsync(customerToCreate, ct);

            await cache.SetKeyAsync(sessionId.ToString(), customer.Id.ToString(), TimeSpan.FromHours(2), ct);

            var response = new JoinRestaurantQueueResponse
            {
                AccessToken = customer.AccessToken
            };

            return response;
        }
    }
}
