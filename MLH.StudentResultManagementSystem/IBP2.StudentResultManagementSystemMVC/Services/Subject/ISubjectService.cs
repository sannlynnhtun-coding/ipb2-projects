using IBP2.StudentResultManagementSystemMVC.Models;

namespace IBP2.StudentResultManagementSystemMVC.Services.Subject
{
    public interface ISubjectService
    {
        Task<CreateSubjectResponse> CreateSubjectAsync(CreateSubjectRequest request);
        Task<GetSubjectResponse?> GetSubjectByIdAsync(int id);
        Task<GetSubjectsResponse> GetSubjectsAsync();
        Task<UpdateSubjectResponse?> UpdateSubjectAsync(UpdateSubjectRequest request);
        Task<bool> DeleteSubjectAsync(int id);
    }
}
