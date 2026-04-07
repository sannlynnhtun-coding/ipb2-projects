using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.MVCApp.Features.Order
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly MenuService _menuService;

        public OrderController(OrderService orderService, MenuService menuService)
        {
            _orderService = orderService;
            _menuService = menuService;
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var response = await _orderService.GetListAsync(new OrderListRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            });
            return View("~/Features/Order/Index.cshtml", response);
        }

        public async Task<IActionResult> Create()
        {
            var menuItems = await _menuService.GetMenuItemListAsync(new MenuItemListRequest { PageNo = 1, PageSize = 100 });
            ViewBag.MenuItems = menuItems.Data;
            return View("~/Features/Order/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreateOrderRequest request)
        {
            await _orderService.CreateAsync(request);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(new DeleteOrderRequest { OrderId = id });
            return RedirectToAction("Index");
        }
    }
}
