using Hangfire;
using notifier.Application.ServiceTests.Command.TestServices;
using notifier.Domain.UnitOfWork;

namespace notifier.Hangfire
{
    public interface IHangfireMethods
    {
        void TestServices(int time);
        Task Run();
    }

    public class HangfireMethods : IHangfireMethods
    {
        private readonly IMediator mediator;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HangfireMethods(
            IMediator mediator, 
            IBackgroundJobClient backgroundJobClient,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.mediator = mediator;
            this.backgroundJobClient = backgroundJobClient;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void TestServices(int times)
        {
            
            RecurringJob.AddOrUpdate("TestServices", () => TestServicesJob(),
                $"*/{times} * * * *",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local
                });
        }

        public async Task Run() 
        {
            using var Scoped = _serviceScopeFactory.CreateScope();
            using var unitOfWork = Scoped.ServiceProvider.GetService<IUnitsOfWorks>();
            var Times = await unitOfWork!.ServiceTestRepo.GetPeriodTime();

            foreach(var time in Times) 
            {
                TestServices(time);
            }
        }
        
        public void TestServicesJob()
        {
            mediator.Send(new TestServicesCommand()).GetAwaiter().GetResult();
        }

        

    }
}
