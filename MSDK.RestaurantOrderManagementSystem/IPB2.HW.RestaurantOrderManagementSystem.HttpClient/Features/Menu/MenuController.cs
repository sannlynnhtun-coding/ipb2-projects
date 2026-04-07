using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.HttpClient.Features.Menu
{
    public class MenuController : Controller
    {
        private readonly HttpClient _httpClient;

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        #region Category

        public async Task<IActionResult> CategoryIndex(int pageNo = 1, int pageSize = 10)
        {
            var url = $"api/Category/List?pageNo={pageNo}&pageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<CategoryListResponse>(url);
            return View("~/Features/Menu/Category/Index.cshtml", response);
        }

        public IActionResult CategoryCreate()
        {
            return View("~/Features/Menu/Category/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CategorySave(CreateCategoryRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/Category", request);
            return RedirectToAction("CategoryIndex");
        }

        public async Task<IActionResult> CategoryEdit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<CategoryDto>>($"api/Category/{id}");
            if (response == null || !response.Success) return NotFound();

            var updateRequest = new UpdateCategoryRequest
            {
                CategoryId = response.Data.CategoryId,
                CategoryName = response.Data.CategoryName
            };
            return View("~/Features/Menu/Category/Edit.cshtml", updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUpdate(UpdateCategoryRequest request)
        {
            await _httpClient.PutAsJsonAsync("api/Category", request);
            return RedirectToAction("CategoryIndex");
        }

        public async Task<IActionResult> CategoryDelete(int id)
        {
            await _httpClient.DeleteAsync($"api/Category/{id}");
            return RedirectToAction("CategoryIndex");
        }

        #endregion

        #region MenuItem

        public async Task<IActionResult> MenuItemIndex(int pageNo = 1, int pageSize = 10)
        {
            var url = $"api/MenuItem/List?pageNo={pageNo}&pageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<MenuItemListResponse>(url);
            return View("~/Features/Menu/MenuItem/Index.cshtml", response);
        }

        public async Task<IActionResult> MenuItemCreate()
        {
            var categoriesResponse = await _httpClient.GetFromJsonAsync<CategoryListResponse>("api/Category/List?pageNo=1&pageSize=100");
            ViewBag.Categories = categoriesResponse.Data;
            return View("~/Features/Menu/MenuItem/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> MenuItemSave(CreateMenuItemRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/MenuItem", request);
            return RedirectToAction("MenuItemIndex");
        }

        public async Task<IActionResult> MenuItemEdit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<MenuItemListResponse>("api/MenuItem/List?pageNo=1&pageSize=100");
            var item = response.Data.FirstOrDefault(x => x.MenuItemId == id);
            if (item == null) return NotFound();

            var categoriesResponse = await _httpClient.GetFromJsonAsync<CategoryListResponse>("api/Category/List?pageNo=1&pageSize=100");
            ViewBag.Categories = categoriesResponse.Data;

            var updateRequest = new UpdateMenuItemRequest
            {
                MenuItemId = item.MenuItemId,
                Name = item.Name,
                Price = item.Price,
                CategoryId = item.CategoryId,
                IsAvailable = item.IsAvailable ?? true
            };
            return View("~/Features/Menu/MenuItem/Edit.cshtml", updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> MenuItemUpdate(UpdateMenuItemRequest request)
        {
            await _httpClient.PutAsJsonAsync("api/MenuItem", request);
            return RedirectToAction("MenuItemIndex");
        }

        public async Task<IActionResult> MenuItemDelete(int id)
        {
            await _httpClient.DeleteAsync($"api/MenuItem/{id}");
            return RedirectToAction("MenuItemIndex");
        }

        #endregion
    }
}
