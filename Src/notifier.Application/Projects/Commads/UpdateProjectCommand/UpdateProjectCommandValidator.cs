namespace notifier.Application.Projects.Commads.UpdateProjectCommand;


public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommandRequest>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Your description is more than 500 charecter.");
    }
}
