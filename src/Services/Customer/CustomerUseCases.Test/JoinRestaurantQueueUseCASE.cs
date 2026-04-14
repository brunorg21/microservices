using Auth.Api.Application.Interfaces;
using Auth.Api.Domain.Cache;
using Auth.Api.Domain.Repositories;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Contracts;
using Moq;
using Shouldly;

namespace CustomerUseCases.Test
{
    public class JoinRestaurantQueueUseCaseTest
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<ICacheRepository> _mockCache;
        private readonly Mock<IRabbitMQPublisher> _publisher;
        private readonly IJoinRestaurantQueueUseCase _useCase;

        public JoinRestaurantQueueUseCaseTest()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockCache = new Mock<ICacheRepository>();
            _publisher = new Mock<IRabbitMQPublisher>();
            _useCase = new Auth.Api.Application.UseCases.JoinRestaurantQueueUseCase(
                _mockCustomerRepository.Object,
                _mockCache.Object, 
                _publisher.Object);
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
                .Setup(x => x.AddAsync(It.IsAny<Auth.Api.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Auth.Api.Domain.Entities.Customer
                {
                    Id = Guid.NewGuid(),
                    AccessToken = Guid.NewGuid().ToString(),
                    Name = "Test Customer",
                    Phone = "1234567890",
                    Seats = 4
                });

            _mockCache
                .Setup(x => x.SetKeyAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()));

            _publisher
                .Setup(x => x.Publish<JoinRestaurantQueueEvent>(It.IsAny<JoinRestaurantQueueEvent>()));
        }
    }
}
