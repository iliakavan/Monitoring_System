namespace notifier.Application.Projects.Commads.AddProjectCommand;







public class AddProjectCommandHandler(IUnitsOfWorks uow) : IRequestHandler<AddProjectCommandRequest,ResultResponse>
{
    private readonly IUnitsOfWorks _unitOfWork = uow;

    public async Task<ResultResponse> Handle(AddProjectCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null) 
        {
            return new() { Success = false };
        }
        var Project = new Project()
        { 
            Title = request.Title,
            Description = request.Description,
            RecordDate = DateTime.Now
        };

        await _unitOfWork.ProjectRepo.Add(Project);
        await _unitOfWork.SaveChanges();
        return new() { Success = true , Message = "Project Created"};
    }
}
