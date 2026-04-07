using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.MVCApp.Features.Report
{
    public class ReportController : Controller
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Now.AddDays(-7);
            var end = endDate ?? DateTime.Now;

            var response = await _service.GetSalesReportAsync(new SalesReportRequest
            {
                StartDate = start,
                EndDate = end
            });

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");

            return View("~/Features/Report/Index.cshtml", response);
        }
    }
}
