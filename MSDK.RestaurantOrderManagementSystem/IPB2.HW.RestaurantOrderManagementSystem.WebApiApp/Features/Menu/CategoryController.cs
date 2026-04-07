using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HW.RestaurantOrderManagementSystem.WebApiApp.Features.Menu
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MenuService _service;

        public CategoryController(MenuService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] CategoryListRequest request)
        {
            var response = await _service.GetListAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetListAsync(new CategoryListRequest
            {
                PageNo = 1,
                PageSize = 1
            });

            var data = item.Data.FirstOrDefault(x => x.CategoryId == id);

            if (data == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Success.",
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var response = await _service.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryRequest request)
        {
            var response = await _service.UpdateAsync(request);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(new DeleteCategoryRequest
            {
                CategoryId = id
            });

            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}
