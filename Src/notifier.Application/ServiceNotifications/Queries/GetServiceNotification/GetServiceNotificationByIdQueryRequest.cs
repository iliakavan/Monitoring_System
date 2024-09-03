namespace notifier.Application.ServiceNotifications.Queries.GetServiceNotification;


public class GetServiceNotificationByIdQueryRequest : IRequest<ResultResponse<ServiceNotificationDto>>
{
    public int Id { get; set; }

}
