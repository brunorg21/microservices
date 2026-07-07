using Auth.Api.Application.Interfaces;
using Auth.Api.Domain.Repositories;
using Auth.Api.Domain.Security.Token;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Contracts;
using Moq;
using Shouldly;

namespace CustomerUseCases.Test
{
    public class JoinRestaurantQueueUseCaseTest
    {
        private readonly Mock<IUserRepository> _mockCustomerRepository;
        private readonly Mock<IRabbitMQPublisher> _publisher;
        private readonly Mock<ITokenGenerator> _mockTokenGenerator;
        private readonly IJoinRestaurantQueueUseCase _useCase;

        public JoinRestaurantQueueUseCaseTest()
        {
            _mockCustomerRepository = new Mock<IUserRepository>();
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _publisher = new Mock<IRabbitMQPublisher>();
            _useCase = new Auth.Api.Application.UseCases.JoinRestaurantQueueUseCase(
                _mockCustomerRepository.Object, 
                _publisher.Object,
                _mockTokenGenerator.Object);
        }

        [Fact(DisplayName = "should be join in restaurant queue")]
        public async Task JoinRestaurantQueue()
        {
            ConfigureMocks();

            var request = new Auth.Api.DTOs.Request.JoinRestaurantQueueRequest
            {
                Name = "Test Customer",
                Phone = "1234567890",
                Seats = 4,
            };

            var result = await _useCase.Execute(request, CancellationToken.None);

            result.ShouldNotBeNull();
            result.AccessToken.ShouldNotBeNullOrEmpty();
        }

        private void ConfigureMocks()
        {
            _mockCustomerRepository
                .Setup(x => x.AddAsync(It.IsAny<Auth.Api.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Auth.Api.Domain.Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Customer",
                    Phone = "1234567890",
                    Seats = 4
                });

            _publisher
                .Setup(x => x.Publish<JoinRestaurantQueueEvent>(It.IsAny<JoinRestaurantQueueEvent>()));

            _mockTokenGenerator.Setup(x => x.GenerateToken(It.IsAny<Auth.Api.Domain.Entities.User>())).ReturnsAsync("mocked-token");
        }
    }
}
