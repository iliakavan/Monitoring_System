using DNTPersianUtils.Core;

namespace notifier.Application.ProjectOfficials.Quries.Search;



public class SearchProjectOfficialQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchProjectOfficialQueryRequest, ResultResponse<IEnumerable<ProjectOfficialDto>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ProjectOfficialDto>>> Handle(SearchProjectOfficialQueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var projectOfficials = await _unitsOfWorks.ProjectOffcialRepo
                .Search(startdateEN, enddateEn,request.Responsible,request.Mobile,request.TelegramId,request.ProjectId);

        if (projectOfficials is null || !projectOfficials.Any()) 
        {
            return new() { Success = false };
        }

        return new() { Success = true, Value = projectOfficials };
    }
}
