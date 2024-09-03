
using DNTPersianUtils.Core;

namespace notifier.Application.ServiceTestLogs.Query.SearchV2;



public class SearchServiceTestLogV2QueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchServiceTestLogV2QueryRequest, ResultResponse<IEnumerable<object>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse<IEnumerable<object>>> Handle(SearchServiceTestLogV2QueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        IEnumerable<object> log = await _unitsOfWorks.ServiceTestLogRepo.Search(startdateEN, enddateEn, request.ServiceId,request.ResponseCode,request.TestType,request.ProjectId,request.Ip,request.Port,request.Url);

        if (log is null || !log.Any())
        {
            return new() { Success = false };
        }

        return new() { Success = true, Value = log };
    }
}

