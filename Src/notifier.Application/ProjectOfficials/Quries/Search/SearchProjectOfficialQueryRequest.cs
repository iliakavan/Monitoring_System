namespace notifier.Application.ProjectOfficials.Quries.Search;



public class SearchProjectOfficialQueryRequest : IRequest<ResultResponse<IEnumerable<ProjectOfficialDto>>>
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Responsible {  get; set; }
    public string? Mobile {  get; set; }
    public string? TelegramId {  get; set; }
    public int? ProjectId { get; set; }
    public SearchProjectOfficialQueryRequest()
    {
        
    }
    public SearchProjectOfficialQueryRequest(string? s, string? e,string? responsible,string? mobile,string? telegramId,int? projectid)
    {
        StartDate = s;
        EndDate = e;
        Responsible = responsible;
        Mobile = mobile;
        TelegramId = telegramId;
        ProjectId = projectid;
    }
}
