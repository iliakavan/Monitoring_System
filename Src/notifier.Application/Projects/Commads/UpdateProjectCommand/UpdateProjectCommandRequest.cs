namespace notifier.Application.Projects.Commads.UpdateProjectCommand;



public class UpdateProjectCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
