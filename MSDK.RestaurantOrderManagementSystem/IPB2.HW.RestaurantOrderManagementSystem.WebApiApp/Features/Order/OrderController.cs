using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.WebApiApp.Features.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] OrderListRequest request)
        {
            var response = await _service.GetListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var response = await _service.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrderRequest request)
        {
            var response = await _service.UpdateAsync(request);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(new DeleteOrderRequest
            {
                OrderId = id
            });

            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}
