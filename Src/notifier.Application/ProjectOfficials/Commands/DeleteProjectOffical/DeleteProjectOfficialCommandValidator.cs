namespace notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;


public class DeleteProjectOfficialCommandValidator : AbstractValidator<DeleteProjectOfficialCommandRequest>
{
    public DeleteProjectOfficialCommandValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("Given ID is not valid");
    }
}
