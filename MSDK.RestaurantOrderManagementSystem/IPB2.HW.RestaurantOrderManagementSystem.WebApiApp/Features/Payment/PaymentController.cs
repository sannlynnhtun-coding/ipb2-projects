using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.WebApiApp.Features.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentController(PaymentService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] PaymentListRequest request)
        {
            var response = await _service.GetListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentRequest request)
        {
            var response = await _service.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePaymentRequest request)
        {
            var response = await _service.UpdateAsync(request);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(new DeletePaymentRequest
            {
                PaymentId = id
            });

            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}
