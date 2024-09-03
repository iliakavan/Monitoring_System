namespace notifier.Application.Services.Commands.DeleteService;


public class DeleteServiceCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }

}
