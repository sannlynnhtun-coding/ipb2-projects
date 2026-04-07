using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineScheduleSystem.WebApi.Features.Admin.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.WebApi.Features.Report
{
    [Route("api/BookingReport")]
    [ApiController]
    public class BookingReportController : ControllerBase
    {
        IBookingService _reportService ;
        public BookingReportController(IBookingService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            //var response = await _reportService.GetBookingDetailAsync();
            return Ok();
        }
    }
}
