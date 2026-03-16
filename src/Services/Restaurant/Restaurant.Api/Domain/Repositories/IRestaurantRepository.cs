namespace Restaurant.Api.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Entities.Restaurant> CreateAsync(Entities.Restaurant restaurant);
    }
}
