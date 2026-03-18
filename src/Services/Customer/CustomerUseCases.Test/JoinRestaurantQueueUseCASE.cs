using Customer.Api.Application.Interfaces;
using Customer.Api.Domain.Cache;
using Customer.Api.Domain.Repositories;
using Moq;
using Shouldly;

namespace CustomerUseCases.Test
{
    public class JoinRestaurantQueueUseCaseTest
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<ICacheRepository> _mockCache;
        private readonly IJoinRestaurantQueueUseCase _useCase;

        public JoinRestaurantQueueUseCaseTest()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockCache = new Mock<ICacheRepository>();
            _useCase = new Customer.Api.Application.UseCases.JoinRestaurantQueueUseCase(_mockCustomerRepository.Object, _mockCache.Object);
        }

        [Fact(DisplayName = "should be join in restaurant queue")]
        public async Task JoinRestaurantQueue()
        {
            ConfigureMocks();

            var request = new Customer.Api.DTOs.Request.JoinRestaurantQueueRequest
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
                .Setup(x => x.AddAsync(It.IsAny<Customer.Api.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Customer.Api.Domain.Entities.Customer
                {
                    Id = Guid.NewGuid(),
                    AccessToken = Guid.NewGuid().ToString(),
                    Name = "Test Customer",
                    Phone = "1234567890",
                    Seats = 4
                });

            _mockCache
                .Setup(x => x.SetKeyAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()));
        }
    }
}
