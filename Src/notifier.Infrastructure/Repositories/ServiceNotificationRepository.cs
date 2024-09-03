

namespace notifier.Infrastructure.Repositories;






public class ServiceNotificationRepository(AppDbcontext context) : IServiceNotificationRepository
{
    private readonly AppDbcontext _context = context;

    public async Task Add(ServiceNotfications model)
    {
        await _context.AddAsync(model);
    }

    public void Delete(ServiceNotfications model)
    {
        model.IsActive = false;
        Update(model);
    }

    public async Task<IEnumerable<ServiceNotfications>> GetAll()
    {
        return await _context.ServiceNotfications.ToListAsync();
    }

    public async Task<ServiceNotfications?> GetById(int id)
    {
        return await _context.ServiceNotfications.Where(S =>  S.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ServiceNotificationDto?>> Search(DateTime? StartDate, DateTime? EndDate,NotificationType? notifeType,int? ServiceTestId,int? ServiceId,int? ProjectId)
    {
        DateTime start = StartDate ?? DateTime.MinValue;
        DateTime end = EndDate ?? DateTime.MaxValue;
        var Query = _context.ServiceNotfications.Include(s => s.ServiceTest).ThenInclude(s => s.Service).ThenInclude(s => s.Project).AsQueryable().Where(x => x.RecordDate >= start && x.RecordDate <= end);
        if(notifeType.HasValue)
        {
            Query = Query.Where(x => x.NotificationType == notifeType);
        }
        if(ServiceTestId.HasValue) 
        {
            Query = Query.Where(x => x.ServiceTestId == ServiceTestId);
        }
        if(ServiceId.HasValue) 
        {
            Query = Query.Where(x => x.ServiceTest.ServiceId == ServiceId);
        }
        if(ProjectId.HasValue) 
        {
            Query = Query.Where(x => x.ServiceTest.Service.ProjectId == ProjectId);
        }
        return await Query.Select(notife => new ServiceNotificationDto() 
        { 
            Id = notife.Id,
            ServiceID = notife.ServiceTest.ServiceId,
            ServiceName = notife.ServiceTest.Service.Title,
            ServiceTestId = notife.ServiceTestId,
            NotificationType = notife.NotificationType.ToString(),
            MessageFormat = notife.MessageFormat,
            MessageSuccess = notife.MessageSuccess,
            ProjectID = notife.ServiceTest.Service.ProjectId,
            ProjectName = notife.ServiceTest.Service.Project.Title,
            RecordDate = notife.RecordDate,
            RetryCount = notife.RetryCount,
            TestType = notife.ServiceTest.TestType.ToString()
        }).ToListAsync();
    }

    public void Update(ServiceNotfications model)
    {
        _context.ServiceNotfications.Update(model);
    }
    public async Task<List<ServiceNotfications>> GetAllServices()
    {
        return await _context.ServiceNotfications
                                            .Include(s => s.ServiceTest)
                                                        .ThenInclude(s => s.Service)
                                                                .ThenInclude(s => s.Project)
                                                                        .ThenInclude(s => s.ProjectOfficials).ToListAsync();
    }
}
