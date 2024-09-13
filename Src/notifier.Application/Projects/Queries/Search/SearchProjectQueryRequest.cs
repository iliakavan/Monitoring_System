namespace notifier.Application.Projects.Queries.Search;


public class SearchProjectQueryRequest : IRequest<ResultResponse<IEnumerable<Project?>>>
{
    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? Title { get; set; }

    public SearchProjectQueryRequest(string? start,string? end, string? title)
    {
        StartDate = start;
        EndDate = end;
        Title = title;
    }
    public SearchProjectQueryRequest()
    {
        
    }
}
