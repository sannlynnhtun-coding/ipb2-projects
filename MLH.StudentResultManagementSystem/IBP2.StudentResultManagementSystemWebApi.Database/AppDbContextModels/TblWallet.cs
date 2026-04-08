using System;
using System.Collections.Generic;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class TblWallet
{
    public string WalletId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool IsDelete { get; set; }
}
