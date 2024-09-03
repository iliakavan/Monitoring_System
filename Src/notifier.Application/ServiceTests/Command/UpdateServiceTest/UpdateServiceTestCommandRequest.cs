namespace notifier.Application.ServiceTests.Command.UpdateServiceTest;




public class UpdateServiceTestCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }
    public int? PriodTime { get; set; }

    public TestType? TestType { get; set; }

    public int? ServiceId { get; set; }
}
