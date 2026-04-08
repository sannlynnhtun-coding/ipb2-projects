namespace IBP2.StudentResultManagementSystemWebApi.Features.Results;

public class GetResultsRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
