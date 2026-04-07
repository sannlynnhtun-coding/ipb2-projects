using IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment
{
    public class PaymentService
    {
        private readonly AppDbContext _db;

        public PaymentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PaymentListResponse> GetListAsync(PaymentListRequest request)
        {
            if (request.PageNo <= 0) request.PageNo = 1;
            if (request.PageSize <= 0) request.PageSize = 10;

            var query = _db.Payments
                .AsNoTracking()
                .OrderByDescending(x => x.PaymentId);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaymentDto
                {
                    PaymentId = x.PaymentId,
                    OrderId = x.OrderId,
                    PaymentDate = x.PaymentDate,
                    PaymentMethod = x.PaymentMethod,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();

            return new PaymentListResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Data = data
            };
        }

        public async Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request)
        {
            var item = new Database.AppDbContextModels.Payment
            {
                OrderId = request.OrderId,
                PaymentDate = DateTime.Now,
                PaymentMethod = request.PaymentMethod,
                TotalAmount = request.TotalAmount
            };

            _db.Payments.Add(item);
            await _db.SaveChangesAsync();

            return new CreatePaymentResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = new PaymentDto
                {
                    PaymentId = item.PaymentId,
                    OrderId = item.OrderId,
                    PaymentDate = item.PaymentDate,
                    PaymentMethod = item.PaymentMethod,
                    TotalAmount = item.TotalAmount
                }
            };
        }

        public async Task<UpdatePaymentResponse> UpdateAsync(UpdatePaymentRequest request)
        {
            var item = await _db.Payments
                .FirstOrDefaultAsync(x => x.PaymentId == request.PaymentId);

            if (item == null)
            {
                return new UpdatePaymentResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            item.PaymentMethod = request.PaymentMethod;
            item.TotalAmount = request.TotalAmount;

            await _db.SaveChangesAsync();

            return new UpdatePaymentResponse
            {
                Success = true,
                Message = "Updated successfully.",
                Data = new PaymentDto
                {
                    PaymentId = item.PaymentId,
                    OrderId = item.OrderId,
                    PaymentDate = item.PaymentDate,
                    PaymentMethod = item.PaymentMethod,
                    TotalAmount = item.TotalAmount
                }
            };
        }

        public async Task<DeletePaymentResponse> DeleteAsync(DeletePaymentRequest request)
        {
            var item = await _db.Payments
                .FirstOrDefaultAsync(x => x.PaymentId == request.PaymentId);

            if (item == null)
            {
                return new DeletePaymentResponse
                {
                    Success = false,
                    Message = "Not found."
                };
            }

            _db.Payments.Remove(item);
            await _db.SaveChangesAsync();

            return new DeletePaymentResponse
            {
                Success = true,
                Message = "Deleted successfully."
            };
        }
    }
}
