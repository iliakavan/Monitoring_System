namespace notifier.Domain.Repositories;


public interface IServiceTestLogRepository
{
    Task Insert(ServiceTestLog log);

    Task<IEnumerable<ServiceTestLog>> Search(DateTime? StartDate, DateTime? EndDate, int? serviceId);

    Task<IEnumerable<object>> Search(DateTime? StartDate, DateTime? EndDate, int? serviceId, string? ResponseCode, TestType? testtype,int? projectId,string? Ip,int? port,string? Url);


}
