namespace notifier.Application.ServiceTestLogs.Query.SearchV1;



public class SearchServiceTestLog : IRequest<ResultResponse<IEnumerable<ServiceTestLog>>>
{
    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public int? Serviceid { get; set; }

    public SearchServiceTestLog()
    {

    }

    public SearchServiceTestLog(string? s, string? e, int? serviceid)
    {
        StartDate = s;
        EndDate = e;
        Serviceid = serviceid;
    }
}
