using IPB2.OnlineBusSystem.Domain.Features.Booking;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.MVC.Controllers
{
    public class BookingAdminController : Controller
    {
        private readonly IBookingService _bookingService;
        public BookingAdminController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BookingAllList(string? txtBkSearch)
        {
            BookingDetailResponse response = new();
            response = await _bookingService.GetBookingDetailAsync(txtBkSearch);
            return View(response);
        }

    }
}
