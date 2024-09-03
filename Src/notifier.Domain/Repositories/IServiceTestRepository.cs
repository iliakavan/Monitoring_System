namespace notifier.Domain.Repositories;



public interface IServiceTestRepository : IRepository<ServiceTest>
{
    Task<IEnumerable<ServiceTestDto>> Search(DateTime? StartDate, DateTime? EndDate, int? ServiceId,int? ProjectId);
}


