namespace notifier.Domain.UnitOfWork;





public interface IUnitsOfWorks : IDisposable
{
    IProjectOffcialRepository ProjectOffcialRepo { get; }
    
    IProjectRepository ProjectRepo{ get; }

    IServiceNotificationRepository NotificationRepo { get; }

    IServiceRepository ServiceRepo { get; }

    IServiceTestRepository ServiceTestRepo { get; }
    IServiceTestLogRepository ServiceTestLogRepo { get; }

    IUserRepository UserRepo { get; }

    Task SaveChanges();
}
