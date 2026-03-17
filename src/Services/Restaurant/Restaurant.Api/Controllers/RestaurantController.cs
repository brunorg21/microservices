using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Application.Interfaces;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
   
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] ICreateRestaurantService createRestaurantService, 
            CreateRestaurantRequest request,
            CancellationToken ct)
        {
            var (statusCode, result) = await createRestaurantService.CreateAsync(request, ct);

            return StatusCode(statusCode, result);
        }
    }
}
