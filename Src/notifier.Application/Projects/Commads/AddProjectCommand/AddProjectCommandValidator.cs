using notifier.Domain.UnitOfWork;

namespace notifier.Application.Projects.Commads.AddProjectCommand;



public class AddProjectCommandValidator : AbstractValidator<AddProjectCommandRequest> 
{
    private readonly IUnitsOfWorks _uow;
    public AddProjectCommandValidator(IUnitsOfWorks uow)
    {
        _uow = uow;
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50).MustAsync(CheckIfProjectDoesnotExist).WithMessage("Project Already Exist.");
        RuleFor(x => x.Description).MaximumLength(500);

    }

    private async Task<bool> CheckIfProjectDoesnotExist(string Title, CancellationToken cancellationToken) 
    {
        var result = await _uow.ProjectRepo.GetProjectByTitleAsync(Title);

        if (result == null) 
        {
            return true;
        }
        return false;
    }
}
