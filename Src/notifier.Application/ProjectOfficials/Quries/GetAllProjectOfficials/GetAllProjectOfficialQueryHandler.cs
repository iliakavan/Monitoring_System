namespace notifier.Application.ProjectOfficials.Quries.GetAllProjectOfficials;




public class GetAllProjectOfficialQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetAllProjectOfficialQueryRequest, ResultResponse<IEnumerable<ProjectOfficial>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ProjectOfficial>>> Handle(GetAllProjectOfficialQueryRequest request, CancellationToken cancellationToken)
    {
        var projectof = await _unitsOfWorks.ProjectOffcialRepo.GetAll();

        if (projectof is null || !projectof.Any()) 
        {
            return new() { Success = false };
        }

        return new() { Success = true , Value = projectof};
    }
}
