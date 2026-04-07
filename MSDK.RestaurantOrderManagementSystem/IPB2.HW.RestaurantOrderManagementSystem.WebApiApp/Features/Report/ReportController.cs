using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.WebApiApp.Features.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("Sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] SalesReportRequest request)
        {
            var response = await _service.GetSalesReportAsync(request);
            return Ok(response);
        }
    }
}
