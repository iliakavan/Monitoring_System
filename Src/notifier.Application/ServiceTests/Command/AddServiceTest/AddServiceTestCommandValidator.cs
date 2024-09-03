using notifier.Domain.Enum;

namespace notifier.Application.ServiceTests.Command.AddServiceTest;



public class AddServiceTestCommandValidator : AbstractValidator<AddServiceTestCommandRequest>
{
    public AddServiceTestCommandValidator()
    {
        RuleFor(S => S.PriodTime).GreaterThan(1);
        RuleFor(S => S.TestType).IsInEnum();
        RuleFor(S => S.ServiceId).NotNull();
    }
}
