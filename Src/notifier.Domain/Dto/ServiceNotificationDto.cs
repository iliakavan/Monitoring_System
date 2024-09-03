namespace notifier.Domain.Dto;



public class ServiceNotificationDto
{
    public int Id { get; set; }
    public int RetryCount { get; set; }

    public int ServiceTestId { get; set; }

    public string NotificationType { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public string MessageFormat { get; set; } = null!;
    public string MessageSuccess {  get; set; } = null!;
    public string TestType { get; set; } = null!;
    public string ServiceName { get; set; } = null!;
    public int ServiceID {  get; set; }
    public string ProjectName { get; set; } = null!;
    public int ProjectID {  get; set; }
}
