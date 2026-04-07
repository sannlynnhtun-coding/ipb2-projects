using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IPB2.OnlineBusSystem.MVC.HttpClientWebApi.Controllers
{
    public class BookingAdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public BookingAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public IActionResult Index()
        {
            return View();
        }
        

    }
}
