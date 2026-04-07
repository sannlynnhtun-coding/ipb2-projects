using System;
using System.Collections.Generic;

namespace IPB2.NorthwindCharts.Database.AppDbContextModels;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
