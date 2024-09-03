namespace notifier.Application.Services.Queries.Search;


public class SearchServiceQueryRequest : IRequest<ResultResponse<IEnumerable<ServiceDto?>>>
{
    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? Url {  get; set; }
    public string? Title {  get; set; }
    public string? Ip {  get; set; }
    public int? Port { get; set; }
    public int? ProjectId { get; set; }


    public SearchServiceQueryRequest()
    {
        
    }

    public SearchServiceQueryRequest(string? s,string? e,string? url,string? title,string? ip,int? port,int? projectId)
    {
        StartDate = s;
        EndDate = e;
        Url = url;
        Title = title;
        Ip = ip;
        Port = port;
        ProjectId = projectId;
    }


}
