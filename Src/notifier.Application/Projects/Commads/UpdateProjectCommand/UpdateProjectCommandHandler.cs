namespace notifier.Application.Projects.Commads.UpdateProjectCommand;



public class UpdateProjectCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UpdateProjectCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(UpdateProjectCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null) 
        {
            return new() { Success = false};
        }

        var project = await _unitsOfWorks.ProjectRepo.GetById(request.Id);
        
        if(project is null) 
        {
            return new() { Success = false, Message = "Project doesnt Exist" };
        }

        project.Title = request.Title ?? project.Title;
        project.Description = request.Description ?? project.Description;
        _unitsOfWorks.ProjectRepo.Update(project);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true, Message = $"{request.Id} Updated Successfully" };
    }
}
