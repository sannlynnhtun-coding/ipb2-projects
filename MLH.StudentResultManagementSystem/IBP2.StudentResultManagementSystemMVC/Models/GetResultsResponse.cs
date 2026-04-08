namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class GetResultsResponse
    {
        public int TotalCount { get; set; }
        public List<ResultModel> Data { get; set; } = new();
    }
}
