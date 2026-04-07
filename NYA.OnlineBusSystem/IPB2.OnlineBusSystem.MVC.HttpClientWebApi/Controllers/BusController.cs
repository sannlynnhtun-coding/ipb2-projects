using Azure;
using IPB2.OnlineBusSystem.Domain.Common;
using IPB2.OnlineBusSystem.Domain.Features.Bus;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace IPB2.OnlineBusSystem.MVC.HttpClientWebApi.Controllers
{
    public class BusController : Controller
    {
        private readonly HttpClient _httpClient;

        public BusController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
        {
            GetBusResponse response = new GetBusResponse();
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/bus?pageNo=1&pageSize=10");
            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<GetBusResponse>(json)!;

            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBusRequest request)
        {
            if (ModelState.IsValid)
            {
                var upsertRequest = new UpsertBusRequest
                {
                    BusNo = request.BusNo,
                    BusName = request.BusName,
                    BusType = request.BusType,
                    TotalSeat = request.TotalSeat
                };

                var requestJson = JsonConvert.SerializeObject(upsertRequest);
                HttpContent content = new StringContent(requestJson,Encoding.UTF8,Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/bus",content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to create bus.");
                }
               
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertBusRequest request, string id)
        {
            if (ModelState.IsValid)
            {
                var requestJson = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(requestJson, Encoding.UTF8, Application.Json);

                HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/bus/{id}", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                    if (response.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", response.Message ?? "Failed to update bus.");
                }
               
            }
            return RedirectToAction(nameof(Index));
        }
            
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/bus/delete/{id}", null);

            if (httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                ResponseBaseModel response = JsonConvert.DeserializeObject<ResponseBaseModel>(json)!;
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.Message ?? "Failed to delete bus.");
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
