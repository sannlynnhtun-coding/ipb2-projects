using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.MVC.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
        {
            var response = await _busService.GetBusAsync(pageNumber, 10);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                response = await _busService.GetBusesBySearchAsync(searchTerm);
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBusRequest request)
        {
            if (ModelState.IsValid)
            {
                var upsertRequest = new UpsertBusRequest
                {
                    BusNo = request.BusNo,
                    BusName = request.BusName,
                    BusType = request.BusType,
                    TotalSeat = request.TotalSeat
                };
                var result = await _busService.CreateAsync(upsertRequest);
                if (result.Status ==ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to create bus.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertBusRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _busService.UpsertAsync(request, id);
                if (result.Status ==ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to update bus.");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _busService.DeleteAsync(id);
            if (result.Status == ResponseType.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message ?? "Failed to delete bus.");
            return RedirectToAction(nameof(Index));
        }
    }
}
