using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.WebApiApp.Features.Kitchen
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly KitchenService _service;

        public KitchenController(KitchenService service)
        {
            _service = service;
        }

        [HttpGet("PendingOrders")]
        public async Task<IActionResult> GetPendingOrders([FromQuery] PendingOrderListRequest request)
        {
            var response = await _service.GetPendingOrdersAsync(request);
            return Ok(response);
        }

        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeOrderStatusRequest request)
        {
            var response = await _service.ChangeOrderStatusAsync(request);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}
