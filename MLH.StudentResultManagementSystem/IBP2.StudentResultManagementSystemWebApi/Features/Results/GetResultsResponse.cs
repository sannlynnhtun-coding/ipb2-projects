using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Results;

public class GetResultsResponse
{
    public int TotalCount { get; set; }
    public List<Result> Data { get; set; } = new();
}
