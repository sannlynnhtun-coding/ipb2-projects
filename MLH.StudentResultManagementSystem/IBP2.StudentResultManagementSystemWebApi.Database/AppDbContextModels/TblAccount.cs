using System;
using System.Collections.Generic;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class TblAccount
{
    public string AccountId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool? IsDelete { get; set; }
}
