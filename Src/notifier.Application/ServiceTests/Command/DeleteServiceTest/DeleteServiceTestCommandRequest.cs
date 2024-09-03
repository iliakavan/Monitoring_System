namespace notifier.Application.ServiceTests.Command.DeleteServiceTest;


public class DeleteServiceTestCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }

}
