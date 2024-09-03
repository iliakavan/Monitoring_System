namespace notifier.Application.ServiceTests.Command.AddServiceTest;





public class AddServiceTestCommandRequest : IRequest<ResultResponse>
{
    public int PriodTime { get; set; }

    public TestType TestType { get; set; }

    public int ServiceId { get; set; }
}
