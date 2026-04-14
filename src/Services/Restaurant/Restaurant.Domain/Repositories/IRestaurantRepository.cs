namespace Restaurant.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Entities.Restaurant> AddAsync(Entities.Restaurant restaurant, CancellationToken ct);
    }
}
