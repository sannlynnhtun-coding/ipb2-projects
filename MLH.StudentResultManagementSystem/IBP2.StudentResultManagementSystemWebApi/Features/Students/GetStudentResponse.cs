using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Students;

public class GetStudentResponse
{
    public Student Data { get; set; } = null!;
}
