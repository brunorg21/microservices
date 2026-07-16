using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantQueueController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CallNextCustomer(
            [FromServices] ICallNextCustomerUseCase useCase,
            [FromQuery] Guid restaurantQueueEntry)
        {
            await useCase.Execute(restaurantQueueEntry);

            return Ok();
        }
    }
}
