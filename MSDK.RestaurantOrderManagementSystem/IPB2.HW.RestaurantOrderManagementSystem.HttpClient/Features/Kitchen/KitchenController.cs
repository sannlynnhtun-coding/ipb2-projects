using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Features.Kitchen
{
    public class KitchenController : Controller
    {
        private readonly HttpClient _httpClient;

        public KitchenController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var url = $"api/Kitchen/PendingOrders?pageNo={pageNo}&pageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<PendingOrderListResponse>(url);
            return View("~/Features/Kitchen/Index.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            var request = new ChangeOrderStatusRequest
            {
                OrderId = orderId,
                Status = status
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), "api/Kitchen/ChangeStatus")
            {
                Content = content
            };

            await _httpClient.SendAsync(requestMessage);
            return RedirectToAction("Index");
        }
    }
}
