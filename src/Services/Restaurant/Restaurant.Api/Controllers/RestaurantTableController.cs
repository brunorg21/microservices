using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;

namespace Restaurant.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantTableController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTables(
            [FromServices] ICreateRestaurantTableUseCase useCase, 
            [FromBody] CreateRestaurantTableListRequest request)
        {
            await useCase.Execute(request);

            return Created();
        }
    }
}
