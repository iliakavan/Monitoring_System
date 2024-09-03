namespace notifier.Application.ServiceTests.Command.DeleteServiceTest;


public class DeleteServiceTestCommandValidator : AbstractValidator<DeleteServiceTestCommandRequest>
{
    public DeleteServiceTestCommandValidator()
    {
        RuleFor(S => S.Id).GreaterThan(0).WithMessage("Given ID is not valid");
    }
}
