using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
