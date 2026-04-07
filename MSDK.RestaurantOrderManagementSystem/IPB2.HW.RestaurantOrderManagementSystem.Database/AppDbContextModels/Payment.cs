using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Order? Order { get; set; }
}
