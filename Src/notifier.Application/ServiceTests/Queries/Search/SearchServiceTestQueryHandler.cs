
using DNTPersianUtils.Core;

namespace notifier.Application.ServiceTests.Queries.Search;



public class SearchServiceTestQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchServiceTestQueryRequest, ResultResponse<IEnumerable<ServiceTestDto>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ServiceTestDto>>> Handle(SearchServiceTestQueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var serviceTest = await _unitsOfWorks.ServiceTestRepo.Search(startdateEN, enddateEn,request.Serviceid,request.Projectid);

        if(serviceTest is null || !serviceTest.Any()) 
        {
            return new() { Success = false, Message = "ServiceTest is Empty Or Null" };
        }
        return new() { Success = true, Value = serviceTest };
    }
}
