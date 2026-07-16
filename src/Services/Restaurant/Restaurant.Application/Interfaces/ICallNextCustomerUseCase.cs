namespace Restaurant.Application.Interfaces
{
    public interface ICallNextCustomerUseCase
    {
        Task Execute(Guid restaurantQueueEntryId);
    }
}
