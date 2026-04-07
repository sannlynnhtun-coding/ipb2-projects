using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order
{
    public class OrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<OrderListResponse> GetListAsync(OrderListRequest request)
        {
            if (request.PageNo <= 0) request.PageNo = 1;
            if (request.PageSize <= 0) request.PageSize = 10;

            var query = _db.Orders
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .OrderByDescending(x => x.OrderId);

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

            return new OrderListResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Data = data
            };
        }

        public async Task<CreateOrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var item = new Database.AppDbContextModels.Order
            {
                OrderDate = DateTime.Now,
                Status = request.Status,
                TableNumber = request.TableNumber,
                OrderItems = request.OrderItems?.Select(x => new Database.AppDbContextModels.OrderItem
                {
                    MenuItemId = x.MenuItemId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList() ?? new List<Database.AppDbContextModels.OrderItem>()
            };

            _db.Orders.Add(item);
            await _db.SaveChangesAsync();

            return new CreateOrderResponse
            {
                Success = true,
                Message = "Created successfully.",
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

        public async Task<UpdateOrderResponse> UpdateAsync(UpdateOrderRequest request)
        {
            var item = await _db.Orders
                .FirstOrDefaultAsync(x => x.OrderId == request.OrderId);

            if (item == null)
            {
                return new UpdateOrderResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            item.Status = request.Status;
            item.TableNumber = request.TableNumber;

            await _db.SaveChangesAsync();

            return new UpdateOrderResponse
            {
                Success = true,
                Message = "Updated successfully.",
                Data = new OrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    Status = item.Status,
                    TableNumber = item.TableNumber
                }
            };
        }

        public async Task<DeleteOrderResponse> DeleteAsync(DeleteOrderRequest request)
        {
            var item = await _db.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.OrderId == request.OrderId);

            if (item == null)
            {
                return new DeleteOrderResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            if (item.OrderItems.Any())
            {
                _db.OrderItems.RemoveRange(item.OrderItems);
            }
            if (item.Payments.Any())
            {
                _db.Payments.RemoveRange(item.Payments);
            }

            _db.Orders.Remove(item);
            await _db.SaveChangesAsync();

            return new DeleteOrderResponse
            {
                Success = true,
                Message = "Deleted successfully."
            };
        }
    }
}
