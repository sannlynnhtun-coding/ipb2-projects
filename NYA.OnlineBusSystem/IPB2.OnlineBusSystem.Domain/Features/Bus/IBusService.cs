using IPB2.OnlineBusSystem.Domain.Common;

namespace IPB2.OnlineBusSystem.Domain.Features.Bus
{
    public interface IBusService
    {
        Task<ServiceResponse> CreateAsync(UpsertBusRequest request);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<GetBusResponse> GetBusAsync(int pageNo, int pageSize);
        Task<BusResponse?> GetBusByIdAsync(string id);
        Task<List<BusComboSetModel>> GetBusComboSet();
        Task<GetBusResponse> GetBusesBySearchAsync(string? searchTerm);
        Task<ServiceResponse> UpdateAsync(UpdateBusRequest request, string id);
        Task<ServiceResponse> UpsertAsync(UpsertBusRequest request, string id);
    }
}