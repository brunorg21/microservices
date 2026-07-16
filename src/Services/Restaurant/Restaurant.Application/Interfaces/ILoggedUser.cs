using Restaurant.Domain.DTOs;

namespace Restaurant.Application.Interfaces
{
    public interface ILoggedUser
    {
        LoggedUserDTO GetLoggedUser();
    }
}
