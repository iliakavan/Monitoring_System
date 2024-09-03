namespace notifier.Application.Projects.Queries.GetById;





public class GetProjectByIdQueryRequest : IRequest<ResultResponse<ProjectDto>>
{
    public int Id { get; set; }

}
