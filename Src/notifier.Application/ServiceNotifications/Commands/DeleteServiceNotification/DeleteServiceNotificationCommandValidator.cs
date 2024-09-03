namespace notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;


public class DeleteServiceNotificationCommandValidator : AbstractValidator<DeleteServiceNotificationCommandRequest>
{
    public DeleteServiceNotificationCommandValidator()
    {
        RuleFor(S => S.Id).GreaterThan(0).WithMessage("Given ID is not valid");
    }
}
