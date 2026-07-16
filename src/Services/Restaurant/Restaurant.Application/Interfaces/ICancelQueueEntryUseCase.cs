namespace Restaurant.Application.Interfaces
{
    public interface ICancelQueueEntryUseCase
    {
        Task Execute(Guid restaurantQueueEntryId);
    }
}
