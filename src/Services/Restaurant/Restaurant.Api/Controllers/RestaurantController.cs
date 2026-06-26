using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
   
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] ICreateRestaurantUseCase useCase, 
            CreateRestaurantRequest request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
