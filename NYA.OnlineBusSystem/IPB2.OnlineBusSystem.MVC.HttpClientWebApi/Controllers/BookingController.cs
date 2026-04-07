using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Booking;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace IPB2.OnlineBusSystem.MVC.HttpClientWebApi.Controllers
{
    public class BookingController : Controller
    {

        private readonly HttpClient _httpClient;

        public BookingController(IHttpClientFactory httpClientFactory)          
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> Index(SearchBusRequest? request)
        {
            SearchBusResponse response = new();            

            // Fetch Routes for dropdown
            HttpResponseMessage routeResponse = await _httpClient.GetAsync("api/routes/comboset");
            if (routeResponse.IsSuccessStatusCode)
            {
                var routeJson = await routeResponse.Content.ReadAsStringAsync();
                var routes = JsonConvert.DeserializeObject<List<RouteComboSetModel>>(routeJson)!;

                ViewBag.Origins = routes.Select(r => r.Origin).Distinct().OrderBy(o => o).ToList();
                ViewBag.Destinations = routes.Select(r => r.Destination).Distinct().OrderBy(d => d).ToList();
            }

            var requestJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

            HttpResponseMessage searchResponse = await _httpClient.PostAsync("api/booking/search", content);

            if (searchResponse.IsSuccessStatusCode)
            {
                var json = await searchResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<SearchBusResponse>(json)!;
                

                ModelState.AddModelError("","Failed to create bus.");
            }

            return View(response);
        }

        public async Task<IActionResult> Book(string scheduleId)
        {
            ScheduleResponse response = new ScheduleResponse();
            if (string.IsNullOrEmpty(scheduleId)) return RedirectToAction(nameof(Index));

            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/schedules/{scheduleId}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ScheduleResponse>(json)!;


                HttpResponseMessage httpBusResponse = await _httpClient.GetAsync($"api/bus/{response.BusId}");
                var jsonBus = await httpBusResponse.Content.ReadAsStringAsync();
                BusResponse bus = JsonConvert.DeserializeObject<BusResponse>(jsonBus)!;

                HttpResponseMessage httpRouteResponse = await _httpClient.GetAsync($"api/routes/{response.RouteId}");
                var jsonRoute = await httpRouteResponse.Content.ReadAsStringAsync();
                RouteResponse route = JsonConvert.DeserializeObject<RouteResponse>(jsonRoute)!;

                ViewBag.Bus = bus;
                ViewBag.Route = route;

            }
            else
            {
                return NotFound();
            }    
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest request)
        {
            if (ModelState.IsValid)
            {
                var url = $"api/booking/book";

                var requestJson = JsonConvert.SerializeObject(request) ;
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json) ;

                HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, content);
                if (httpResponse.IsSuccessStatusCode)
                {

                    return Json(new { success = true, message = "Booking successful!" });
                }

                return Json(new { success = false, message = "Booking failed." });
            }
            return Json(new { success = false, message = "Invalid booking data." });
        }
        public async Task<IActionResult> BookingList(string? username, string? phoneno)
        {
            var url = $"api/booking/getBookinglist?username={username ?? ""}&phoneno={phoneno ?? ""}";
            BookingDetailResponse response = new();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(phoneno))
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<BookingDetailResponse>(json)!;
                    
                }
            }
            return View(response);
        }

        public async Task<IActionResult> BookingAllList(string? txtBkSearch)
        {
            BookingDetailResponse response = new BookingDetailResponse();

            var url = $"api/booking/getAllBookinglist?txtBkSearch={txtBkSearch ?? ""}";
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BookingDetailResponse>(json)!;
                if (result != null)
                {
                    response = result;
                }

            }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(string id)
        {
            HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/booking/cancel/{id}", null);

            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookingAllList));
                }

                ModelState.AddModelError("", response.Message ?? "Failed to cancel booking.");
            }


            return RedirectToAction(nameof(BookingAllList));
        }
    }
}
