namespace notifier.Application.Projects.Commads.DeleteProjectCommand;



public class DeleteProjectCommandHandler(IUnitsOfWorks uow) : IRequestHandler<DeleteProjectCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null) 
        {
            return new ResultResponse() { Success = false};
        }

        var project = await _unitsOfWorks.ProjectRepo.GetById(request.Id);

        if(project is null) 
        {
            return new() { Message = "Project doesn't exist.", Success = false };
        }

        _unitsOfWorks.ProjectRepo.Delete(project);
        await _unitsOfWorks.SaveChanges();
        return new ResultResponse() { Success = true , Message = $"{project.Title} Deleted Successfully"};
    }
}
