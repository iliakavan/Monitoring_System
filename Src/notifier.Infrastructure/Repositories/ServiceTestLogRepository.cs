

namespace notifier.Infrastructure.Repositories;



public class ServiceTestLogRepository(AppDbcontext context) : IServiceTestLogRepository
{
    private readonly AppDbcontext _context = context;
    public async Task Insert(ServiceTestLog log)
    {
        await _context.ServiceTestsLogs.AddAsync(log);
    }

    public async Task<IEnumerable<ServiceTestLog>> Search(DateTime? StartDate, DateTime? EndDate, int? serviceId)
    {
        DateTime start = StartDate ?? DateTime.MinValue;
        
        DateTime end = EndDate ?? DateTime.MaxValue;
        
        var query = _context.ServiceTestsLogs.AsQueryable().Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);
        
        if (serviceId.HasValue) 
        {
            query = query.Where(x => x.ServiceId == serviceId);
        }
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<object>> Search(DateTime? StartDate, DateTime? EndDate, int? serviceId, string? ResponseCode, TestType? testtype, int? projectId, string? Ip, int? port, string? Url)
    {
        DateTime Start = StartDate ?? DateTime.MinValue;
        DateTime End = EndDate ?? DateTime.MaxValue;

        var query = _context.ServiceTestsLogs
                            .IgnoreQueryFilters()
                            .Where(x => x.RecordDate.Date >= Start && x.RecordDate.Date <= End);

        if (!string.IsNullOrEmpty(ResponseCode))
        {
            query = query.Where(x => x.ResponseCode == ResponseCode);
        }

        if (serviceId.HasValue)
        {
            query = query.Where(x => x.ServiceId == serviceId);
        }

        if (!string.IsNullOrEmpty(Url))
        {
            query = query.Where(x => x.Service.Url == Url);
        }

        if (!string.IsNullOrEmpty(Ip))
        {
            query = query.Where(x => x.Service.Ip == Ip);
        }

        if (port.HasValue)
        {
            query = query.Where(x => x.Service.Port == port);
        }

        if (projectId.HasValue)
        {
            query = query.Where(x => x.Service.ProjectId == projectId);
        }

        if (testtype.HasValue)
        {
            query = query.Where(x => x.Service.Tests.Any(t => t.TestType == testtype));
        }

        var result = await query.Include(x => x.Service)
                                .ThenInclude(s => s.Tests)
                                .Select(x => new
                                {
                                    ProjectName = x.Service.Project.Title, // Assuming Project has a Title property
                                    RecordDate = x.RecordDate,
                                    TestType = x.ServiceNotification.ServiceTest.TestType.ToString(),
                                    ResponseCode = x.ResponseCode,
                                    Url = x.Service.Url,
                                    Ip = x.Service.Ip,
                                    Port = x.Service.Port
                                })
                                .ToListAsync();

        return result;
    }
}
