
using DNTPersianUtils.Core;



namespace notifier.Application.ServiceNotifications.Queries.Search;



public class SearchServiceNotificationQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchServiceNotificationQueryRequest , ResultResponse<IEnumerable<ServiceNotificationDto?>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse<IEnumerable<ServiceNotificationDto?>>> Handle(SearchServiceNotificationQueryRequest request, CancellationToken cancellationToken)
    {
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();
        var service = await _unitsOfWorks.NotificationRepo.Search(startdateEN, enddateEn, request.NotifeType,request.ServicetestId, request.ServiceId,request.ProjectId);

        if (service is null || !service.Any())
        {
            return new() { Success = false, Message = $"There is no Service Notification between {request.StartDate} {request.EndDate}" };
        }

        return new() { Success = true, Value = service };
    }
}
