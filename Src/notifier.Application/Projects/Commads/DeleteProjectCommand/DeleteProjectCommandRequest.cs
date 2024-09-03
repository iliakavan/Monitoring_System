namespace notifier.Application.Projects.Commads.DeleteProjectCommand;


public class DeleteProjectCommandRequest : IRequest<ResultResponse>
{
    public int Id {  get; set; }

}
