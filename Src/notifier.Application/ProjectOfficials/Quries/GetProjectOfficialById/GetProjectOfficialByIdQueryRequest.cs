namespace notifier.Application.ProjectOfficials.Quries.GetProjectOfficialById;



public class GetProjectOfficialByIdQueryRequest : IRequest<ResultResponse<ProjectOfficialDto>>
{
    public int Id {  get; set; }
}
