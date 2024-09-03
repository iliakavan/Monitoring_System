namespace notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;



public class AddProjectOfficialCommandHandler(IUnitsOfWorks uow) : IRequestHandler<AddProjectOfficialCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitOfWorks = uow;
    public async Task<ResultResponse> Handle(AddProjectOfficialCommandRequest request, CancellationToken cancellationToken)
    {
        var Official = new ProjectOfficial()
        { 
            Mobile = request.Mobile,
            Responsible = request.Responsible,
            TelegramId = request.TelegramId,
            RecordDate = DateTime.Now,
            ProjectId = request.ProjectId
        };
        await _unitOfWorks.ProjectOffcialRepo.Add(Official);
        await _unitOfWorks.SaveChanges();

        return new() { Message = "Project Official Added To Project" ,Success = true};

    }
}
