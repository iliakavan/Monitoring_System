namespace notifier.Application.ProjectOfficials.Commands.UpdateProjectOffical;



public class UpdateProjectOfficialCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UpdateProjectOfficialCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(UpdateProjectOfficialCommandRequest request, CancellationToken cancellationToken)
    {
        var ProjectOfficial = await _unitsOfWorks.ProjectOffcialRepo.GetById(request.Id);

        if (ProjectOfficial == null) 
        {
            return new() { Success = false };
        }

        ProjectOfficial.ProjectId = request.ProjectId ?? ProjectOfficial.ProjectId;
        ProjectOfficial.Responsible = request.Responsible ?? ProjectOfficial.Responsible;
        ProjectOfficial.TelegramId = request.TelegramId ?? ProjectOfficial.TelegramId;
        ProjectOfficial.Mobile = request.Mobile ?? ProjectOfficial.Mobile;
        _unitsOfWorks.ProjectOffcialRepo.Update(ProjectOfficial);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true };
    }
}
