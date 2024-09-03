namespace notifier.Application.ServiceNotifications.Queries.GetAllServiceNotification;





public class GetAllServiceNotificationQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetAllServiceNotificationQueryRequest, ResultResponse<IEnumerable<ServiceNotfications>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ServiceNotfications>>> Handle(GetAllServiceNotificationQueryRequest request, CancellationToken cancellationToken)
    {
        var serviceN = await _unitsOfWorks.NotificationRepo.GetAll();

        if (serviceN is null || !serviceN.Any()) 
        {
            return new() { Success = false };
        }
        return new() { Success = true, Value = serviceN };
    }
}
