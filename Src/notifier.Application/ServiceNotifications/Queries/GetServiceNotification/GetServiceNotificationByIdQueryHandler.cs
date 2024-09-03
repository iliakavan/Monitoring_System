
namespace notifier.Application.ServiceNotifications.Queries.GetServiceNotification;



public class GetServiceNotificationByIdQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetServiceNotificationByIdQueryRequest, ResultResponse<ServiceNotificationDto>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<ServiceNotificationDto>> Handle(GetServiceNotificationByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.NotificationRepo.GetById(request.Id);

        if(service == null) 
        {
            return new() { Success = false };
        }

        ServiceNotificationDto serviceNotificationDto = new()
        {
            MessageFormat = service.MessageFormat,
            NotificationType = service.NotificationType.ToString(),
            RecordDate = service.RecordDate,
            ServiceTestId = service.ServiceTestId,
            RetryCount = service.RetryCount
        };

        return new() { Success = true ,Value = serviceNotificationDto };
        
    }
}
