using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Features.Report
{
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Now.AddDays(-7);
            var end = endDate ?? DateTime.Now;

            var url = $"api/Report/Sales?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}";
            var response = await _httpClient.GetFromJsonAsync<SalesReportResponse>(url);

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");

            return View("~/Features/Report/Index.cshtml", response);
        }
    }
}
