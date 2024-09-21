namespace notifier.Application.ServiceNotifications.Commands.AddServiceNotification;



public class AddServiceNotificationCommandValidator : AbstractValidator<AddServiceNotificationCommandRequest>
{
    public AddServiceNotificationCommandValidator()
    {
        RuleFor(N => N.MaxRetryCount).GreaterThan(0);
        RuleFor(N => N.ServiceTestId).GreaterThan(0);
        RuleFor(N => N.NotificationType).IsInEnum();
        RuleFor(N => N.MessageFormat).NotEmpty();
    }
}
