namespace notifier.Application.Projects.Queries.GetAllProjects;




public class GetAllProjectQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetAllProjectQueryRequest, ResultResponse<IEnumerable<Project?>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<Project?>>> Handle(GetAllProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var project = await _unitsOfWorks.ProjectRepo.GetAll();

        if( project is null || !project.Any()) 
        {
            return new() { Success = false, Message = "Failed to fetch" };
        }
        return new() { Success = true, Value = project };
    }
}
