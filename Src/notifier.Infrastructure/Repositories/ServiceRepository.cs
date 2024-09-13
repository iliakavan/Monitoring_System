using DNTPersianUtils.Core;

namespace notifier.Infrastructure.Repositories;










public class ServiceRepository(AppDbcontext context) : IServiceRepository
{
    private readonly AppDbcontext _context = context;
    public async Task Add(Service model)
    {
        await _context.Services.AddAsync(model);
    }

    public void Delete(Service model)
    {
        if(model.Tests is not null && model.Tests.Count > 0) 
        {
            foreach (var test in model.Tests) 
            {
                if(test.ServiceNotifications is not null &&  test.ServiceNotifications.Count > 0) 
                {
                    foreach(var notification in test.ServiceNotifications) 
                    {
                        notification.IsActive = false;
                    }
                }
                test.IsActive = false;
            }
        }
        model.IsActive = false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<IEnumerable<Service>> GetAll()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<Service?> GetById(int id)
    {
        return await _context.Services.Where(S => S.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Service?> GetByIdIncludeAll(int id)
    {
        return await _context.Services.Where(S => S.Id == id).Include(S => S.Tests).ThenInclude(T => T.ServiceNotifications).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ServiceDto?>> Search(DateTime? StartDate, DateTime? EndDate,string? Title,string? url,string? Ip,int? Port,int? projectId)
    {
        DateTime start = StartDate ?? DateTime.MinValue;
        DateTime end = EndDate ?? DateTime.MaxValue;

        var query = _context.Services.AsQueryable().Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);
        

        if (!string.IsNullOrEmpty(Title))
        {
            query = query.Where(x => x.Title == Title);
        }
        // Apply Url filter if Url is provided
        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(x => x.Url == url);
        }

        // Apply Ip filter if Ip is provided
        if (!string.IsNullOrEmpty(Ip))
        {
            query = query.Where(x => x.Ip == Ip);
        }

        // Apply Port filter if Port is provided
        if (Port.HasValue)
        {
            query = query.Where(x => x.Port == Port.Value);
        }

        // Apply ProjectId filter if ProjectId is provided
        if (projectId.HasValue)
        {
            query = query.Where(x => x.ProjectId == projectId.Value);
        }

        return await query.Select(service => new ServiceDto
        {
            Id = service.Id,
            Title = service.Title,
            RecordDate = service.RecordDate,
            Url = service.Url,
            Ip = service.Ip,
            Port = service.Port,
            ProjectName = service.Project.Title, // Map the Project title
            ProjectId = service.Project.Id,
            Method = service.Method
        }).ToListAsync();
    }
    public void Update(Service model)
    {
        _context.Services.Update(model);
    }
}
