namespace notifier.Application.Projects.Commads.AddProjectCommand;





public class AddProjectCommandRequest : IRequest<ResultResponse>
{
    public required string Title { get; set; }

    public string? Description { get; set; }
}
