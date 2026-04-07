using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace IPB2.OnlineBusSystem.MVC.HttpClientWebApi.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly HttpClient _httpClient;

        public ScheduleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> Index(string? searchDate)
        {
            if (string.IsNullOrEmpty(searchDate))
            {
                searchDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            GetScheduleListResponse response = new GetScheduleListResponse();
            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/schedules/searchByDate?searchDate={searchDate}");
            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<GetScheduleListResponse>(json)!;
            }

            // Fetch Buses for dropdown
            HttpResponseMessage busResponse = await _httpClient.GetAsync("api/bus/comboset");
            if (busResponse.IsSuccessStatusCode)
            {
                var busJson = await busResponse.Content.ReadAsStringAsync();
                ViewBag.Buses = JsonConvert.DeserializeObject<List<BusComboSetModel>>(busJson);
            }

            // Fetch Routes for dropdown
            HttpResponseMessage routeResponse = await _httpClient.GetAsync("api/routes/comboset");
            if (routeResponse.IsSuccessStatusCode)
            {
                var routeJson = await routeResponse.Content.ReadAsStringAsync();
                ViewBag.Routes = JsonConvert.DeserializeObject<List<RouteComboSetModel>>(routeJson);
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpsertScheduleRequest request)
        {
            if (ModelState.IsValid)
            {
                var requestJson = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/schedules", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to create schedule.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertScheduleRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var requestJson = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/schedules/{id}", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to update schedule.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/schedules/delete/{id}", null);

            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.Message ?? "Failed to delete schedule.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
