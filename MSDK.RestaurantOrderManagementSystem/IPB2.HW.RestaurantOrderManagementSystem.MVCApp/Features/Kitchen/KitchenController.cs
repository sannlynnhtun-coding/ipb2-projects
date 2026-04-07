using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.MVCApp.Features.Kitchen
{
    public class KitchenController : Controller
    {
        private readonly KitchenService _service;

        public KitchenController(KitchenService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var response = await _service.GetPendingOrdersAsync(new PendingOrderListRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            });
            return View("~/Features/Kitchen/Index.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await _service.ChangeOrderStatusAsync(new ChangeOrderStatusRequest
            {
                OrderId = orderId,
                Status = status
            });
            return RedirectToAction("Index");
        }
    }
}
