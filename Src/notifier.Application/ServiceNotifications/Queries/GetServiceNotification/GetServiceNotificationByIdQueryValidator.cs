namespace notifier.Application.ServiceNotifications.Queries.GetServiceNotification;



public class GetServiceNotificationByIdQueryValidator : AbstractValidator<GetServiceNotificationByIdQueryRequest>
{
    public GetServiceNotificationByIdQueryValidator()
    {
        RuleFor(s => s.Id).GreaterThan(0);
    }
}
