namespace IBP2.StudentResultManagementSystemWebApi.Features.Students;

public class CreateStudentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool? IsDelete { get; set; }
}
