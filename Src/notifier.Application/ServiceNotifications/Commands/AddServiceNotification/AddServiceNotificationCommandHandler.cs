
namespace notifier.Application.ServiceNotifications.Commands.AddServiceNotification;


public class AddServiceNotificationCommandHandler(IUnitsOfWorks uow) : IRequestHandler<AddServiceNotificationCommandRequest , ResultResponse> 
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse> Handle(AddServiceNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var servicenotfi = new ServiceNotfications()
        {
            RecordDate = DateTime.Now,
            MessageFormat = request.MessageFormat,
            MaxRetryCount = request.MaxRetryCount,
            NotificationType = request.NotificationType,
            ServiceTestId = request.ServiceTestId,
            MessageSuccess = request.MessageSuccess
        };

        await _unitsOfWorks.NotificationRepo.Add(servicenotfi);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true };
    }
}
