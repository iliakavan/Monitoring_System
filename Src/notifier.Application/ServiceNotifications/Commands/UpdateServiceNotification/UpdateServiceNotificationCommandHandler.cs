
namespace notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;


public class UpdateServiceNotificationCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UpdateServiceNotificationCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(UpdateServiceNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var Notifi = await _unitsOfWorks.NotificationRepo.GetById(request.Id);

        if (Notifi == null) 
        {
            return new ResultResponse() { Success = false };
        }

        Notifi.ServiceTestId = request.ServiceTestId ?? Notifi.ServiceTestId;
        Notifi.MessageFormat = request.MessageFormat ?? Notifi.MessageFormat;
        Notifi.MessageSuccess = request.MessageSuccess ?? Notifi.MessageSuccess;
        Notifi.NotificationType = request.NotificationType ?? Notifi.NotificationType;
        Notifi.RetryCount = request.RetryCount ?? Notifi.RetryCount;

        _unitsOfWorks.NotificationRepo.Update(Notifi);
        await _unitsOfWorks.SaveChanges();

        return new ResultResponse() { Success = true };
    }
}
