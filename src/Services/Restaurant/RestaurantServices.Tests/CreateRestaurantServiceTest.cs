using Moq;
using Restaurant.Application.Interfaces;
using Restaurant.Application.UseCases;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.Repositories;
using Shouldly;

namespace RestaurantServices.Tests
{
    public class CreateRestaurantServiceTests
    {
        private readonly ICreateRestaurantUseCase _createRestaurantUseCase;
        private readonly Mock<IRestaurantRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUow;

        public CreateRestaurantServiceTests()
        {
            _mockRepository = new Mock<IRestaurantRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _createRestaurantUseCase = new CreateRestaurantUseCase(_mockRepository.Object, _mockUow.Object);

            _mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Restaurant.Domain.Entities.Restaurant>()))
                .ReturnsAsync(new Restaurant.Domain.Entities.Restaurant
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Restaurant"
                });
            _mockUow.Setup(x => x.CommitAsync());
        }

        [Fact(DisplayName = "should be create a restaurant")]
        public async Task ShouldBeCreateRestaurant()
        {
            var name = "Vitinho's Restaurant";

            var response = await _createRestaurantUseCase.Execute(new CreateRestaurantRequest { Name = name });

            response.ShouldNotBeNull();
        }
    }
}
