namespace notifier.Application.Projects.Queries.GetById;







public class GetProjectByIdQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetProjectByIdQueryRequest, ResultResponse<ProjectDto>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<ProjectDto>> Handle(GetProjectByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var project = await _unitsOfWorks.ProjectRepo.GetById(request.Id);

        if (project is null)
        {
            return new() { Message = "Project doesn't exist.", Success = false };
        }
        var projectDto = new ProjectDto()
        {
            Title = project.Title,
            Description = project.Description
        };

        return new() { Success = true, Value = projectDto };
    }
}