using Auth.Api.Application.Interfaces;
using Auth.Api.Domain.Cache;
using Auth.Api.Domain.Repositories;
using Auth.Api.DTOs.Request;
using Auth.Api.DTOs.Responses;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Contracts;

namespace Auth.Api.Application.UseCases
{
    public class JoinRestaurantQueueUseCase
        (
            ICustomerRepository customerRepository,
            ICacheRepository cache,
            IRabbitMQPublisher publisher
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

            await cache.SetKeyAsync(
                sessionId.ToString(), 
                customer.Id.ToString(), 
                TimeSpan.FromHours(24), 
                ct);

            var message = new JoinRestaurantQueueEvent
            {
                CustomerId = customer.Id,
                AccessToken = sessionId,
                CustomerName = request.Name
            };

            await publisher.Publish(message);

            var response = new JoinRestaurantQueueResponse
            {
                AccessToken = customer.AccessToken
            };

            return response;
        }
    }
}
