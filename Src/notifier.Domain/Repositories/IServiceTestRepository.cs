namespace notifier.Domain.Repositories;



public interface IServiceTestRepository : IRepository<ServiceTest> , IDisposable
{
    Task<IEnumerable<ServiceTestDto>> Search(DateTime? StartDate, DateTime? EndDate, int? ServiceId,int? ProjectId);
    Task<ServiceTest?> GetByIdincludeAll(int ID);
    Task<IEnumerable<ServiceTestID_PeriodDto>> GetPeriodTime();

}


