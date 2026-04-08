namespace IBP2.StudentResultManagementSystemWebApi.Features.Results;

public class CreateResultRequest
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public double Marks { get; set; }
}
