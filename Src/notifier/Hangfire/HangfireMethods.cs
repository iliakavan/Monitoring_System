using Hangfire;
using notifier.Application.ServiceTests.Command.TestServices;

namespace notifier.Hangfire
{
    public interface IHangfireMethods
    {
        void TestServices();
    }

    public class HangfireMethods : IHangfireMethods
    {
        private readonly IMediator mediator;
        private readonly IBackgroundJobClient backgroundJobClient;

        public HangfireMethods(IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            this.mediator = mediator;
            this.backgroundJobClient = backgroundJobClient;
        }
        public void TestServices()
        {
            RecurringJob.AddOrUpdate("TestServices", () => TestServicesJob(),
                "*/10 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local
                });
        }

        public void TestServicesJob()
        {
            mediator.Send(new TestServicesCommand()).GetAwaiter().GetResult();
        }


    }
}
