namespace notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;




public class DeleteServiceNotificationCommandHandler(IUnitsOfWorks uow) : IRequestHandler<DeleteServiceNotificationCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse> Handle(DeleteServiceNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var Notifi = await _unitsOfWorks.NotificationRepo.GetById(request.Id);

        if (Notifi == null) 
        {
            return new() { Success = false };
        }

        _unitsOfWorks.NotificationRepo.Delete(Notifi);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true };
    }
}
