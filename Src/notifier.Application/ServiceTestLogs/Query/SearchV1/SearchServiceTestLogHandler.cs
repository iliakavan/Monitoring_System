
using DNTPersianUtils.Core;

namespace notifier.Application.ServiceTestLogs.Query.SearchV1;


public class SearchServiceTestLogHandler(IUnitsOfWorks uow) : IRequestHandler<SearchServiceTestLog, ResultResponse<IEnumerable<ServiceTestLog>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ServiceTestLog>>> Handle(SearchServiceTestLog request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        IEnumerable<ServiceTestLog> log = await _unitsOfWorks.ServiceTestLogRepo.Search(startdateEN, enddateEn, request.Serviceid);

        if (log is null || !log.Any())
        {
            return new() { Success = false };
        }

        return new() { Success = true, Value = log };
    }
}
