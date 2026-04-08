using System;
using System.Collections.Generic;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
