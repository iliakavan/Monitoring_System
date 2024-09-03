
namespace notifier.Application.ProjectOfficials.Quries.GetProjectOfficialById;



public class GetProjectOfficialByIdQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetProjectOfficialByIdQueryRequest, ResultResponse<ProjectOfficialDto>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<ProjectOfficialDto>> Handle(GetProjectOfficialByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var projectOf = await _unitsOfWorks.ProjectOffcialRepo.GetById(request.Id);

        if (projectOf == null) 
        {
            return new() { Success = false };
        }

        ProjectOfficialDto official = new()
        { 
            Mobile = projectOf.Mobile,
            Responsible = projectOf.Responsible,
            TelegramId = projectOf.TelegramId,
            RecordDate = projectOf.RecordDate,
            ProjectId = projectOf.ProjectId
        };
        return new() { Success = true , Value = official};
    }
}
