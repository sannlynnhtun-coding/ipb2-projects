using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Domain.Features.Payment
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class PaymentListRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class PaymentListResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<PaymentDto> Data { get; set; }
    }

    public class CreatePaymentRequest
    {
        public int? OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class CreatePaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PaymentDto Data { get; set; }
    }

    public class UpdatePaymentRequest
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class UpdatePaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PaymentDto Data { get; set; }
    }

    public class DeletePaymentRequest
    {
        public int PaymentId { get; set; }
    }

    public class DeletePaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
