using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public int? TableNumber { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
