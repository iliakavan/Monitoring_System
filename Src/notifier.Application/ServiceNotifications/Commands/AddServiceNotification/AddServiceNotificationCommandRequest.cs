namespace notifier.Application.ServiceNotifications.Commands.AddServiceNotification;


public class AddServiceNotificationCommandRequest : IRequest<ResultResponse>
{
    public int MaxRetryCount { get; set; }
    public int ServiceTestId { get; set; }
    public NotificationType NotificationType { get; set; }
    public required string MessageFormat { get; set; }
    public required string MessageSuccess {  get; set; }
}
