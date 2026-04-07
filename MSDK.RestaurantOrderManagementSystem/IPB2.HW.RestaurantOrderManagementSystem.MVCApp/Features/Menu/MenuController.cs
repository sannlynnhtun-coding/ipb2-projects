using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.MVCApp.Features.Menu
{
    public class MenuController : Controller
    {
        private readonly MenuService _service;

        public MenuController(MenuService service)
        {
            _service = service;
        }

        #region Category

        public async Task<IActionResult> CategoryIndex(int pageNo = 1, int pageSize = 10)
        {
            var response = await _service.GetListAsync(new CategoryListRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            });
            return View("~/Features/Menu/Category/Index.cshtml", response);
        }

        public IActionResult CategoryCreate()
        {
            return View("~/Features/Menu/Category/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CategorySave(CreateCategoryRequest request)
        {
            await _service.CreateAsync(request);
            return RedirectToAction("CategoryIndex");
        }

        public async Task<IActionResult> CategoryEdit(int id)
        {
            var response = await _service.GetListAsync(new CategoryListRequest { PageNo = 1, PageSize = 100 });
            var item = response.Data.FirstOrDefault(x => x.CategoryId == id);
            if (item == null) return NotFound();

            var updateRequest = new UpdateCategoryRequest
            {
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName
            };
            return View("~/Features/Menu/Category/Edit.cshtml", updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUpdate(UpdateCategoryRequest request)
        {
            await _service.UpdateAsync(request);
            return RedirectToAction("CategoryIndex");
        }

        public async Task<IActionResult> CategoryDelete(int id)
        {
            await _service.DeleteAsync(new DeleteCategoryRequest { CategoryId = id });
            return RedirectToAction("CategoryIndex");
        }

        #endregion

        #region MenuItem

        public async Task<IActionResult> MenuItemIndex(int pageNo = 1, int pageSize = 10)
        {
            var response = await _service.GetMenuItemListAsync(new MenuItemListRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            });
            return View("~/Features/Menu/MenuItem/Index.cshtml", response);
        }

        public async Task<IActionResult> MenuItemCreate()
        {
            var categories = await _service.GetListAsync(new CategoryListRequest { PageNo = 1, PageSize = 100 });
            ViewBag.Categories = categories.Data;
            return View("~/Features/Menu/MenuItem/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> MenuItemSave(CreateMenuItemRequest request)
        {
            await _service.CreateMenuItemAsync(request);
            return RedirectToAction("MenuItemIndex");
        }

        public async Task<IActionResult> MenuItemEdit(int id)
        {
            var response = await _service.GetMenuItemListAsync(new MenuItemListRequest { PageNo = 1, PageSize = 100 });
            var item = response.Data.FirstOrDefault(x => x.MenuItemId == id);
            if (item == null) return NotFound();

            var categories = await _service.GetListAsync(new CategoryListRequest { PageNo = 1, PageSize = 100 });
            ViewBag.Categories = categories.Data;

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
            await _service.UpdateMenuItemAsync(request);
            return RedirectToAction("MenuItemIndex");
        }

        public async Task<IActionResult> MenuItemDelete(int id)
        {
            await _service.DeleteMenuItemAsync(new DeleteMenuItemRequest { MenuItemId = id });
            return RedirectToAction("MenuItemIndex");
        }

        #endregion
    }
}
