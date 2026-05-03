using System;
using System.Collections.Generic;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class BlogDetail
{
    public int BlogDetailId { get; set; }

    public int? BlogId { get; set; }

    public string BlogContent { get; set; } = null!;

    public virtual BlogHeader? Blog { get; set; }
}
