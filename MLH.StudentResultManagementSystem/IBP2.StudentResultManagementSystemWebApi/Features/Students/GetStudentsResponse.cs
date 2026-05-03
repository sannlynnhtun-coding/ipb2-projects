using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Students;

public class GetStudentsResponse
{
    public List<Student> Data { get; set; } = new();
}
