using IBP2.StudentResultManagementSystemMVC.Models;

namespace IBP2.StudentResultManagementSystemMVC.Services.Result
{
    public interface IResultService
    {
        Task<CreateResultResponse> CreateResultAsync(CreateResultRequest request);
        Task<ResultModel?> GetResultByIdAsync(int id);
        Task<GetResultsResponse> GetResultsAsync(int pageNo, int pageSize);
        Task<UpdateResultResponse?> UpdateResultAsync(UpdateResultRequest request);
    }
}
