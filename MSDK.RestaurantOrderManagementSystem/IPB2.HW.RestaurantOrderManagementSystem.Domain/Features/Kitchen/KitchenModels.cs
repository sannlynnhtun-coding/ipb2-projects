using IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Order;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Kitchen
{
    public class PendingOrderListRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class PendingOrderListResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<OrderDto> Data { get; set; }
    }

    public class ChangeOrderStatusRequest
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }

    public class ChangeOrderStatusResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OrderDto Data { get; set; }
    }
}
