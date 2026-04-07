namespace IPB2.OnlineBusSystem.Domain.Common;

public class ResponseBaseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
}
