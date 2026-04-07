using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Features.Payment
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            var url = $"api/Payment/List?pageNo={pageNo}&pageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<PaymentListResponse>(url);
            return View("~/Features/Payment/Index.cshtml", response);
        }

        public async Task<IActionResult> Create(int orderId)
        {
            var orderResponse = await _httpClient.GetFromJsonAsync<OrderListResponse>("api/Order/List?pageNo=1&pageSize=100");
            var order = orderResponse.Data.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null) return NotFound();

            var totalAmount = order.OrderItems?.Sum(oi => oi.Price * oi.Quantity) ?? 0;

            var request = new CreatePaymentRequest
            {
                OrderId = orderId,
                TotalAmount = totalAmount
            };
            return View("~/Features/Payment/Create.cshtml", request);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreatePaymentRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/Payment", request);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/Payment/{id}");
            return RedirectToAction("Index");
        }
    }
}
