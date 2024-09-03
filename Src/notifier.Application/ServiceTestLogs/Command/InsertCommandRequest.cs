namespace notifier.Application.SeriveTestLogs.Command;


public class InsertCommandRequest : IRequest<ResultResponse>
{
    public int ServiceId { get; set; }
    public DateTime RecordDate { get; set; }
    public required string ResponseCode { get; set; }
}
