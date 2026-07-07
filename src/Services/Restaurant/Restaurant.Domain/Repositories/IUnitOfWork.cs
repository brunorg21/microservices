namespace Restaurant.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
