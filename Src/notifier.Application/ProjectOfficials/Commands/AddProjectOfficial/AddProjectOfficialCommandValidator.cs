namespace notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;



public class AddProjectOfficialCommandValidator : AbstractValidator<AddProjectOfficialCommandRequest>
{
    public AddProjectOfficialCommandValidator()
    {
        RuleFor(p => p.Mobile)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(11)
            .Matches(@"^09\d{9}$").WithMessage("Phone number is not valid.");
        RuleFor(p => p.Responsible).MaximumLength(50).WithMessage("Given value is more than 50 charachter.");
        RuleFor(p => p.TelegramId).MaximumLength(25);
        RuleFor(p => p.ProjectId).GreaterThan(0);
    }
}
