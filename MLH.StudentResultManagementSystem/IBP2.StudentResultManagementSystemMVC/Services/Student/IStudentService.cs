using IBP2.StudentResultManagementSystemMVC.Models;

namespace IBP2.StudentResultManagementSystemMVC.Services.Student
{
    public interface IStudentService
    {
        Task<CreateStudentResponse> CreateStudentAsync(CreateStudentRequest request);
        Task<GetStudentResponse?> GetStudentByIdAsync(int id);
        Task<GetStudentsResponse> GetStudentsAsync();
        Task<UpdateStudentResponse?> UpdateStudentAsync(UpdateStudentRequest request);
        Task<bool> DeleteStudentAsync(int id);
    }
}
