namespace notifier.Application.ServiceNotifications.Queries.Search;




public class SearchServiceNotificationQueryRequest : IRequest<ResultResponse<IEnumerable<ServiceNotificationDto?>>>
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public NotificationType? NotifeType {  get; set; }
    public int? ServiceId { get; set; }

    public int? ServicetestId { get;set; }

    public int? ProjectId {  get; set; }

    public SearchServiceNotificationQueryRequest()
    {
        
    }
    public SearchServiceNotificationQueryRequest(string? s, string? e,NotificationType? t,int? serviceid,int? servicetestId,int? projectId)
    {
        StartDate = s;
        EndDate = e;
        NotifeType = t;
        ServiceId = serviceid;
        ServicetestId = servicetestId;
        ProjectId = projectId;
    }
}
