namespace notifier.Application.ServiceTests.Queries.Search;



public class SearchServiceTestQueryRequest : IRequest<ResultResponse<IEnumerable<ServiceTestDto>>>
{
    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public int? Serviceid { get; set; }
    public int? Projectid { get; set; }

    public SearchServiceTestQueryRequest(string? start,string? End,int? serviceid,int? projectid) 
    {
        StartDate = start;
        EndDate = End;
        Serviceid = serviceid;
        Projectid = projectid;
    }
    public SearchServiceTestQueryRequest()
    {
        
    }
}
