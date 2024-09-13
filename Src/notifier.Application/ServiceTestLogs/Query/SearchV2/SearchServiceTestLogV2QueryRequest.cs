namespace notifier.Application.ServiceTestLogs.Query.SearchV2;


public class SearchServiceTestLogV2QueryRequest : IRequest<ResultResponse<IEnumerable<object>>>
{
    public int? ProjectId {  get; set; }

    public int? ServiceId {  get; set; }
    public string? ResponseCode {  get; set; }

    public TestType? TestType { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? Ip {  get; set; }

    public int? Port { get; set; }

    public string? Url { get; set; }

    public SearchServiceTestLogV2QueryRequest(string? startDate, string? endDate, int? serviceId, string? responseCode, TestType? testtype, int? projectId, string? ip, int? port, string? url)
    {
        StartDate = startDate;
        EndDate = endDate;
        Port = port;
        ServiceId = serviceId;
        ResponseCode = responseCode;
        TestType = testtype;
        ProjectId = projectId;
        Url = url;
        Ip = ip;
    }
    public SearchServiceTestLogV2QueryRequest()
    {
        
    }
}


