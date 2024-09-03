namespace notifier.Application.Projects.Commads.DeleteProjectCommand;



public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommandRequest>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().GreaterThan(0).WithMessage("Given Id is Not Valid");
    }
}
