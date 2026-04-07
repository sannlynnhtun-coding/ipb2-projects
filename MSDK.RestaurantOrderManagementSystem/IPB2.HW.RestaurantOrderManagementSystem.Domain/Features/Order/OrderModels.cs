using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }
        public int? TableNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int? MenuItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

    public class OrderListRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class OrderListResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<OrderDto> Data { get; set; }
    }

    public class CreateOrderRequest
    {
        public string Status { get; set; }
        public int? TableNumber { get; set; }
        public List<CreateOrderItemRequest> OrderItems { get; set; }
    }

    public class CreateOrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OrderDto Data { get; set; }
    }

    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public int? TableNumber { get; set; }
    }

    public class UpdateOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OrderDto Data { get; set; }
    }

    public class DeleteOrderRequest
    {
        public int OrderId { get; set; }
    }

    public class DeleteOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
