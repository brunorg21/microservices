using Auth.Api.Application.Interfaces;
using Auth.Api.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> JoinQueue(
            [FromServices] IJoinRestaurantQueueUseCase useCase,
            JoinRestaurantQueueRequest request,
            CancellationToken ct,
            HttpContext ctx) 
        { 
            var response = await useCase.Execute(request, ct);

            return Created(string.Empty, response);
        }

    }
}
