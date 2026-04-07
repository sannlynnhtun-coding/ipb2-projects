using System;
using System.Collections.Generic;

namespace IPB2.HW.RestaurantOrderManagementSystem.Database.AppDbContextModels;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
