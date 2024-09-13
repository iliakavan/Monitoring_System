namespace notifier.Domain.Repositories;









public interface IServiceNotificationRepository : IRepository<ServiceNotfications> , IDisposable
{
    Task<IEnumerable<ServiceNotificationDto?>> Search(DateTime? StartDate, DateTime? EndDate, NotificationType? notifeType, int? ServiceTestId, int? ServiceId, int? ProjectId);
    Task<List<ServiceNotfications>> GetAllServices();
}
