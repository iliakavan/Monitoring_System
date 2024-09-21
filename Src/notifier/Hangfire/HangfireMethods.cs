using notifier.Application.ServiceTests.Command.TestServices;
using notifier.Domain.Dto;
using notifier.Domain.UnitOfWork;


namespace notifier.Hangfire;


public interface IHangfireMethods
{
    void TestServices();
    void Run();

}

public class HangfireMethods : IHangfireMethods
{
    private readonly IMediator mediator;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireMethods(
        IMediator mediator, 
        IRecurringJobManager recurringJobManager,
        IServiceScopeFactory serviceScopeFactory)
    {
        this.mediator = mediator;
        _serviceScopeFactory = serviceScopeFactory;
        _recurringJobManager = recurringJobManager;
    }
    public void TestServices()
    {
        var jobid = $"Service Test";
        _recurringJobManager.AddOrUpdateDynamic(jobid, () => TestServicesJob(),
            $"*/1 * * * *",
            new DynamicRecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });
    }

    public void Run() 
    {
        TestServices();
    }

    public async Task TestServicesJob()
    {
        await mediator.Send(new TestServicesCommand());
    }
}
