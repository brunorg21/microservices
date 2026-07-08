using Auth.Api.Application.Interfaces;
using Auth.Api.Domain.Repositories;
using Auth.Api.Domain.Security.Token;
using Auth.Api.DTOs.Request;
using Auth.Api.DTOs.Responses;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Contracts;

namespace Auth.Api.Application.UseCases
{
    public class JoinRestaurantQueueUseCase
        (
            IUserRepository userRepository,
            IRabbitMQPublisher publisher,
            ITokenGenerator tokenGenerator
        ): IJoinRestaurantQueueUseCase
    {
        public async Task<JoinRestaurantQueueResponse> Execute(JoinRestaurantQueueRequest request, CancellationToken ct)
        {
            var customerToCreate = new Domain.Entities.User
            {
                Name = request.Name,
                Phone = request.Phone,
                Seats = request.Seats
            };

            var user = await userRepository.AddAsync(customerToCreate, ct);

            var message = new JoinRestaurantQueueEvent
            {
                CustomerId = user.Id,
                RestaurantId = request.RestaurantId
            };

            await publisher.Publish<JoinRestaurantQueueEvent>(message);
            
            var accessToken = await tokenGenerator.GenerateToken(user);

            var response = new JoinRestaurantQueueResponse
            {
                CustomerId = user.Id,
                AccessToken = accessToken
            };

            return response;
        }
    }
}
