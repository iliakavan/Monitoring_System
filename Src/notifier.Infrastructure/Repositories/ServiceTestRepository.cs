namespace notifier.Infrastructure.Repositories;





public class ServiceTestRepository(AppDbcontext context) : IServiceTestRepository
{
    private readonly AppDbcontext _context = context;

    public async Task Add(ServiceTest model)
    {
        await _context.ServiceTests.AddAsync(model);
    }

    public void Delete(ServiceTest model)
    {
        if (model is null) return;

        model.IsActive = false;

        if (model.ServiceNotifications is not null &&  model.ServiceNotifications.Count > 0)
        {
            foreach (var notification in model.ServiceNotifications) 
            {
                notification.IsActive = false;
            }
        }
        Update(model);
    }

    public async Task<IEnumerable<ServiceTest>> GetAll()
    {
        return await _context.ServiceTests.Include(x => x.ServiceNotifications).ToListAsync();
    }

    public async Task<ServiceTest?> GetById(int id)
    {
        return await _context.ServiceTests.Where(S => S.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ServiceTestDto>> Search(DateTime? StartDate, DateTime? EndDate,int? ServiceId,int? ProjectId)
    {
        DateTime start = StartDate ?? DateTime.MinValue;

        DateTime end = EndDate ?? DateTime.MaxValue;
        
        var query = _context.ServiceTests.Include(S => S.Service).ThenInclude(S => S.Project).Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);
        
        if(ServiceId is not null) 
        {
            query = query.Where(x => x.ServiceId == ServiceId);
        }
        if(ProjectId is not null) 
        {
            query = query.Where(x => x.Service.ProjectId == ProjectId);
        }

        return await query.Select(st => new ServiceTestDto()
        {
            ServiceId = st.ServiceId,
            ServiceName = st.Service.Title,
            PriodTime = st.PriodTime,
            RecordDate = st.RecordDate,
            TestType = st.TestType.ToString(),
            ProjectName = st.Service.Project.Title,
            ProjectID = st.Service.ProjectId,
            Id = st.Id
        }).ToListAsync();
    }

    public void Update(ServiceTest model)
    {
        _context.ServiceTests.Update(model);
    }


}
