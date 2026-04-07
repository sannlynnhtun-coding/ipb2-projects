using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace IPB2.OnlineBusSystem.MVC.HttpClientWebApi.Controllers
{
    public class RouteController : Controller
    {
        private readonly HttpClient _httpClient;

        public RouteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
        {
            GetRoutesResponse response = new GetRoutesResponse();
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/routes?pageNo=1&pageSize=10");
            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<GetRoutesResponse>(json)!;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteRequest request)
        {
            if (ModelState.IsValid)
            {
                var upsertRequest = new UpsertRouteRequest
                {
                    RouteName = request.RouteName,
                    Origin = request.Origin,
                    Destination = request.Destination
                };

                var requestJson = JsonConvert.SerializeObject(upsertRequest);
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/routes", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to create route.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertRouteRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var requestJson = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/routes/{id}", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to update route.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/routes/delete/{id}", null);

            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.Message ?? "Failed to delete route.");
            }


            return RedirectToAction(nameof(Index));
        }
        

    }
}
