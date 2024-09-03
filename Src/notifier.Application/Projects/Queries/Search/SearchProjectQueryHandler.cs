using DNTPersianUtils.Core;

namespace notifier.Application.Projects.Queries.Search;




public class SearchProjectQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchProjectQueryRequest, ResultResponse<IEnumerable<Project>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<Project>>> Handle(SearchProjectQueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();
        var project = await _unitsOfWorks.ProjectRepo.Search(startdateEN,enddateEn, request.Title);

        if(project is null || !project.Any())
        {
            return new() { Success = false , Message = $"There is no Project between {request.StartDate} {request.EndDate} or with the title {request.Title}" };
        }
        return new() { Success = true , Value = project };
    }
}
