using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Domain.DTOs.Requests;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantTableController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTables([FromBody] List<CreateRestaurantTableRequest> request)
        {

            return StatusCode(201);
        }
    }
}
