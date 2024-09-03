namespace notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;


public class UpdateServiceNotificationCommandValidator : AbstractValidator<UpdateServiceNotificationCommandRequest>
{
    public UpdateServiceNotificationCommandValidator()
    {
        RuleFor(S => S.Id).GreaterThan(0).WithMessage("Given ID is not valid");
        RuleFor(N => N.RetryCount).GreaterThan(0);
        RuleFor(N => N.ServiceTestId).GreaterThan(0);
        RuleFor(N => N.NotificationType).IsInEnum();
        RuleFor(N => N.MessageFormat).NotEmpty();
    }
}
