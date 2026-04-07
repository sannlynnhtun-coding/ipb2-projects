using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRouteService _routeService;
        private readonly IScheduleService _scheduleService;
        private readonly IBusService _busService;

        public BookingController(IBookingService bookingService, IRouteService routeService, IScheduleService scheduleService, IBusService busService)
        {
            _bookingService = bookingService;
            _routeService = routeService;
            _scheduleService = scheduleService;
            _busService = busService;
        }

        public async Task<IActionResult> Index(SearchBusRequest? request)
        {
            SearchBusResponse response = new();
            
            // Populate Dropdowns
            var routes = await _routeService.GetRouteComboSet();
            ViewBag.Origins = routes.Select(r => r.Origin).Distinct().OrderBy(o => o).ToList();
            ViewBag.Destinations = routes.Select(r => r.Destination).Distinct().OrderBy(d => d).ToList();

            if (request != null && !string.IsNullOrEmpty(request.Origin) && !string.IsNullOrEmpty(request.Destination))
            {
                response = await _bookingService.SearchBus(request);
            }
            
            return View(response);
        }

        public async Task<IActionResult> Book(string scheduleId)
        {
            if (string.IsNullOrEmpty(scheduleId)) return RedirectToAction(nameof(Index));

            var schedule = await _scheduleService.GetScheduleByIdAsync(scheduleId);
            if (schedule == null) return NotFound();

            var bus = await _busService.GetBusByIdAsync(schedule.BusId);
            var route = await _routeService.GetRouteByIdAsync(schedule.RouteId);

            ViewBag.Bus = bus;
            ViewBag.Route = route;

            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookingService.CreateAsync(request);
                if (result.Status == ResponseType.Success)
                {
                    return Json(new { success = true, message = "Booking successful!" });
                }
                return Json(new { success = false, message = result.Message ?? "Booking failed." });
            }
            return Json(new { success = false, message = "Invalid booking data." });
        }
        public async Task<IActionResult> BookingList(string? username, string? phoneno)
        {
            BookingDetailResponse response = new();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(phoneno))
            {
                response = await _bookingService.GetBookingDetailByUserAsync(username, phoneno);
            }
            return View(response);
        }
       
        [HttpPost]
        public async Task<IActionResult> Cancel(string id)
        {
            var result = await _bookingService.CancelAsync(id);
            if (result.Status == ResponseType.Success)
            {
                return Json(new { success = true, message = result.Message });
            }
            return Json(new { success = false, message = result.Message });
        }
    }
}
