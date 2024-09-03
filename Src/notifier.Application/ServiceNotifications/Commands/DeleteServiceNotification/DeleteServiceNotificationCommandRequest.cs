namespace notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;



public class DeleteServiceNotificationCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }
}
