using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Menu
{
    public class MenuService
    {
        private readonly AppDbContext _db;

        public MenuService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CategoryListResponse> GetListAsync(CategoryListRequest request)
        {
            if (request.PageNo <= 0) request.PageNo = 1;
            if (request.PageSize <= 0) request.PageSize = 10;

            var query = _db.Categories
                .AsNoTracking()
                .OrderBy(x => x.CategoryId);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryDto
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName
                })
                .ToListAsync();

            return new CategoryListResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Data = data
            };
        }

        public async Task<CreateCategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            var item = new Category
            {
                CategoryName = request.CategoryName
            };

            _db.Categories.Add(item);
            await _db.SaveChangesAsync();

            return new CreateCategoryResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = new CategoryDto
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName
                }
            };
        }
        public async Task<UpdateCategoryResponse> UpdateAsync(UpdateCategoryRequest request)
        {
            var item = await _db.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId);

            if (item == null)
            {
                return new UpdateCategoryResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            item.CategoryName = request.CategoryName;

            await _db.SaveChangesAsync();

            return new UpdateCategoryResponse
            {
                Success = true,
                Message = "Updated successfully.",
                Data = new CategoryDto
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName
                }
            };
        }
        public async Task<DeleteCategoryResponse> DeleteAsync(DeleteCategoryRequest request)
        {
            var item = await _db.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId);

            if (item == null)
            {
                return new DeleteCategoryResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            _db.Categories.Remove(item);
            await _db.SaveChangesAsync();

            return new DeleteCategoryResponse
            {
                Success = true,
                Message = "Deleted successfully."
            };
        }

        public async Task<MenuItemListResponse> GetMenuItemListAsync(MenuItemListRequest request)
        {
            if (request.PageNo <= 0) request.PageNo = 1;
            if (request.PageSize <= 0) request.PageSize = 10;

            var query = _db.MenuItems
                .Include(x => x.Category)
                .AsNoTracking()
                .OrderBy(x => x.MenuItemId);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MenuItemDto
                {
                    MenuItemId = x.MenuItemId,
                    Name = x.Name,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.CategoryName : null,
                    IsAvailable = x.IsAvailable
                })
                .ToListAsync();

            return new MenuItemListResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Data = data
            };
        }

        public async Task<CreateMenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest request)
        {
            var item = new MenuItem
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                IsAvailable = request.IsAvailable ?? true
            };

            _db.MenuItems.Add(item);
            await _db.SaveChangesAsync();

            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == item.CategoryId);

            return new CreateMenuItemResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = new MenuItemDto
                {
                    MenuItemId = item.MenuItemId,
                    Name = item.Name,
                    Price = item.Price,
                    CategoryId = item.CategoryId,
                    CategoryName = category != null ? category.CategoryName : null,
                    IsAvailable = item.IsAvailable
                }
            };
        }

        public async Task<UpdateMenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest request)
        {
            var item = await _db.MenuItems
                .FirstOrDefaultAsync(x => x.MenuItemId == request.MenuItemId);

            if (item == null)
            {
                return new UpdateMenuItemResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            item.Name = request.Name;
            item.Price = request.Price;
            item.CategoryId = request.CategoryId;
            item.IsAvailable = request.IsAvailable;

            await _db.SaveChangesAsync();

            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == item.CategoryId);

            return new UpdateMenuItemResponse
            {
                Success = true,
                Message = "Updated successfully.",
                Data = new MenuItemDto
                {
                    MenuItemId = item.MenuItemId,
                    Name = item.Name,
                    Price = item.Price,
                    CategoryId = item.CategoryId,
                    CategoryName = category != null ? category.CategoryName : null,
                    IsAvailable = item.IsAvailable
                }
            };
        }

        public async Task<DeleteMenuItemResponse> DeleteMenuItemAsync(DeleteMenuItemRequest request)
        {
            var item = await _db.MenuItems
                .FirstOrDefaultAsync(x => x.MenuItemId == request.MenuItemId);

            if (item == null)
            {
                return new DeleteMenuItemResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            _db.MenuItems.Remove(item);
            await _db.SaveChangesAsync();

            return new DeleteMenuItemResponse
            {
                Success = true,
                Message = "Deleted successfully."
            };
        }
    }
}
