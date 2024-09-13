

namespace notifier.Domain.Repositories;










public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<ServiceDto?>> Search(DateTime? StartDate, DateTime? EndDate, string? Title, string? url, string? Ip, int? Port, int? projectId);
    Task<Service?> GetByIdIncludeAll(int id);
}
