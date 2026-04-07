using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryListRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class DeleteCategoryRequest
    {
        public int CategoryId { get; set; }
    }

    public class CategoryListResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<CategoryDto> Data { get; set; }
    }

    public class CreateCategoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CategoryDto Data { get; set; }
    }

    public class UpdateCategoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CategoryDto Data { get; set; }
    }

    public class DeleteCategoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class MenuItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public class MenuItemListRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class CreateMenuItemRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public class UpdateMenuItemRequest
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public class DeleteMenuItemRequest
    {
        public int MenuItemId { get; set; }
    }

    public class MenuItemListResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<MenuItemDto> Data { get; set; }
    }

    public class CreateMenuItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MenuItemDto Data { get; set; }
    }

    public class UpdateMenuItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MenuItemDto Data { get; set; }
    }

    public class DeleteMenuItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
