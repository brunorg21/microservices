using Moq;
using Restaurant.Api.Application.Interfaces;
using Restaurant.Api.Application.Services;
using Restaurant.Api.Domain.Repositories;
using Restaurant.Api.DTOs;
using Shouldly;

namespace RestaurantServices.Tests
{
    public class CreateRestaurantServiceTests
    {
        private readonly ICreateRestaurantService _createRestaurantService;
        private readonly Mock<IRestaurantRepository> _mockRepository;

        public CreateRestaurantServiceTests()
        {
            _mockRepository = new Mock<IRestaurantRepository>();
            _createRestaurantService = new CreateRestaurantService(_mockRepository.Object);

            _mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Restaurant.Api.Domain.Entities.Restaurant>(), CancellationToken.None))
                .ReturnsAsync(new Restaurant.Api.Domain.Entities.Restaurant
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Restaurant"
                });
        }

        [Fact]
        public async Task ShouldBeCreateRestaurant()
        {
            var name = "Vitinho's Restaurant";

            var (statusCode, result) = await _createRestaurantService.CreateAsync(new CreateRestaurantRequest
            {
                Name = name
            }, CancellationToken.None);

            statusCode.ShouldBe(201);
            result.ShouldNotBeNull();
        }
    }
}
