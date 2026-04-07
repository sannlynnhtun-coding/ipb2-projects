using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen
{
    public class KitchenService
    {
        private readonly AppDbContext _db;

        public KitchenService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PendingOrderListResponse> GetPendingOrdersAsync(PendingOrderListRequest request)
        {
            if (request.PageNo <= 0) request.PageNo = 1;
            if (request.PageSize <= 0) request.PageSize = 10;

            var query = _db.Orders
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .Where(x => x.Status != "Completed" && x.Status != "Cancelled")
                .OrderBy(x => x.OrderId);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderDto
                {
                    OrderId = x.OrderId,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    TableNumber = x.TableNumber,
                    OrderItems = x.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        MenuItemId = oi.MenuItemId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                })
                .ToListAsync();

            return new PendingOrderListResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Data = data
            };
        }

        public async Task<ChangeOrderStatusResponse> ChangeOrderStatusAsync(ChangeOrderStatusRequest request)
        {
            var item = await _db.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.OrderId == request.OrderId);

            if (item == null)
            {
                return new ChangeOrderStatusResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            item.Status = request.Status;
            await _db.SaveChangesAsync();

            return new ChangeOrderStatusResponse
            {
                Success = true,
                Message = "Status updated successfully.",
                Data = new OrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    Status = item.Status,
                    TableNumber = item.TableNumber,
                    OrderItems = item.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        MenuItemId = oi.MenuItemId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                }
            };
        }
    }
}
