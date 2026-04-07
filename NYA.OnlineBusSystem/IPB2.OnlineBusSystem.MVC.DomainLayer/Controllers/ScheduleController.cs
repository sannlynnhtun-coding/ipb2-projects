using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.MVC.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IBusService _busService;
        private readonly IRouteService _routeService;

        public ScheduleController(IScheduleService scheduleService, IBusService busService, IRouteService routeService)
        {
            _scheduleService = scheduleService;
            _busService = busService;
            _routeService = routeService;
        }

        public async Task<IActionResult> Index(string? searchDate)
        {
            if (string.IsNullOrEmpty(searchDate))
            {
                searchDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var response = await _scheduleService.GetScheduleAsync(searchDate);
            ViewBag.Buses = await _busService.GetBusComboSet();
            ViewBag.Routes = await _routeService.GetRouteComboSet();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpsertScheduleRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _scheduleService.CreateAsync(request);
                if (result.Status == ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to create schedule.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertScheduleRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _scheduleService.UpsertAsync(request, id);
                if (result.Status == ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to update schedule.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _scheduleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
