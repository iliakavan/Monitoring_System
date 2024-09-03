namespace notifier.Application.ServiceTests.Command.UpdateServiceTest;


public class UpdateServiceTestCommandValidator : AbstractValidator<UpdateServiceTestCommandRequest>
{
    public UpdateServiceTestCommandValidator()
    {
        RuleFor(S => S.PriodTime).GreaterThan(1);
        RuleFor(S => S.TestType).IsInEnum();
        RuleFor(S => S.ServiceId).NotNull();
    }
}
