using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBusSystem.MVC.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }
        public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
        {
            var response = await _routeService.GetRoutesAsync(pageNumber, 10);
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
            //    response = await _routeService.GetBusesBySearchAsync(searchTerm);
            //}
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteRequest request)
        {
            if (ModelState.IsValid)
            {
                var upsertRequest = new UpsertRouteRequest
                {
                    RouteName = request.RouteName,
                    Origin = request.Origin,
                    Destination = request.Destination
                };
                var result = await _routeService.CreateAsync(upsertRequest);
                if (result.Status == ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to create route.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertRouteRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _routeService.UpsertAsync(request, id);
                if (result.Status == ResponseType.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message ?? "Failed to update route.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _routeService.DeleteAsync(id);
            if (result.Status == ResponseType.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message ?? "Failed to delete route.");
            return RedirectToAction(nameof(Index));
        }



    }
}
