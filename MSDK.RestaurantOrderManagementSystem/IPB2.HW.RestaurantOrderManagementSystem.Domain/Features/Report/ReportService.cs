using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Report
{
    public class ReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<SalesReportResponse> GetSalesReportAsync(SalesReportRequest request)
        {
            var query = _db.Orders
                .Include(x => x.Payments)
                .AsNoTracking()
                .Where(x => (x.OrderDate >= request.StartDate && x.OrderDate <= request.EndDate) && (x.Status != "Cancelled"));

            var result = await query.ToListAsync();

            var data = result
                .GroupBy(x => x.OrderDate.Value.Date)
                .Select(g => new SalesReportDto
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.SelectMany(o => o.Payments).Sum(p => p.TotalAmount ?? 0)
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            return new SalesReportResponse
            {
                Success = true,
                Message = "Sales report generated successfully.",
                Data = data
            };
        }
    }
}
