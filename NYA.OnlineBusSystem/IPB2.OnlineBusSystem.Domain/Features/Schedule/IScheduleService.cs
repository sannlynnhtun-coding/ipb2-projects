using IPB2.OnlineBusSystem.Domain.Common;

namespace IPB2.OnlineBusSystem.Domain.Features.Schedule
{
    public interface IScheduleService
    {
        Task<ServiceResponse> CreateAsync(UpsertScheduleRequest request);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<GetScheduleResponse> GetScheduleAsync(int pageNo, int pageSize);
        Task<GetScheduleListResponse> GetScheduleAsync(string? searchDate);
        Task<ScheduleResponse?> GetScheduleByIdAsync(string id);
        Task<ServiceResponse> UpdateAsync(UpdateScheduleRequest request, string id);
        Task<ServiceResponse> UpsertAsync(UpsertScheduleRequest request, string id);
    }
}