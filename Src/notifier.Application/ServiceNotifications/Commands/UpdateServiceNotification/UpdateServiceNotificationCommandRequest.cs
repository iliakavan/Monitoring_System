namespace notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;


public class UpdateServiceNotificationCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }
    public int? RetryCount { get; set; }
    public int? ServiceTestId { get; set; }
    public NotificationType? NotificationType { get; set; }
    public string? MessageFormat { get; set; }
    public string? MessageSuccess {  get; set; }
}
