using System;
using System.Collections.Generic;

namespace IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

public partial class Result
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public double Marks { get; set; }

    public string Grade { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
