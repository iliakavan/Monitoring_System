namespace notifier.Domain.Models;

[Table("ServiceTestLogs", Schema = "dbo")]

public class ServiceTestLog
{
    public long Id { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public DateTime RecordDate { get; set; }
    public int ServiceNotificationId {  get; set; }
    public ServiceNotfications ServiceNotification { get; set; } = null!;
    public required string ResponseCode { get; set; }
}
