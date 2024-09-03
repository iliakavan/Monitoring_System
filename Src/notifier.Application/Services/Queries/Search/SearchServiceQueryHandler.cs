using DNTPersianUtils.Core;


namespace notifier.Application.Services.Queries.Search;




public class SearchServiceQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchServiceQueryRequest,ResultResponse<IEnumerable<ServiceDto?>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse<IEnumerable<ServiceDto?>>> Handle(SearchServiceQueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var service = await _unitsOfWorks.ServiceRepo.Search(startdateEN, enddateEn,request.Title,request.Url,request.Ip,request.Port,request.ProjectId);

        if (service == null || !service.Any())
        {
            return new() { Success = false, Message = $"There is no Service between {request.StartDate} {request.EndDate}" };
        }

        return new() { Success = true,Value = service };
        
    }
}
