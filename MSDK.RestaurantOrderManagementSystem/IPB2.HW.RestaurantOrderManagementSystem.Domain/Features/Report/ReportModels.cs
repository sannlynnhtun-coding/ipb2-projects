using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report
{
    public class SalesReportRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SalesReportDto
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class SalesReportResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<SalesReportDto> Data { get; set; }
    }
}
