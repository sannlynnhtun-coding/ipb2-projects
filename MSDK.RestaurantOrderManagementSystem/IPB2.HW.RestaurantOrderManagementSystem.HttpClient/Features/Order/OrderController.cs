using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Features.Order
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var url = $"api/Order/List?pageNo={pageNo}&pageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<OrderListResponse>(url);
            return View("~/Features/Order/Index.cshtml", response);
        }

        public async Task<IActionResult> Create()
        {
            var menuItemsUrl = "api/MenuItem/List?pageNo=1&pageSize=100";
            var menuItemsResponse = await _httpClient.GetFromJsonAsync<MenuItemListResponse>(menuItemsUrl);
            ViewBag.MenuItems = menuItemsResponse.Data;
            return View("~/Features/Order/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreateOrderRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/Order", request);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/Order/{id}");
            return RedirectToAction("Index");
        }
    }
}
