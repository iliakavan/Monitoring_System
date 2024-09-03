namespace notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;


public class DeleteProjectOfficialCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }

}
