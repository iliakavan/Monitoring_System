
namespace notifier.Infrastructure.Repositories;




public class ProjectRepository(AppDbcontext context) : IProjectRepository
{
    private readonly AppDbcontext _context = context;
    public async Task Add(Project model)
    {
        await _context.Projects.AddAsync(model);
    }

    public void Delete(Project model)
    {
        if (model == null) return;

        if (model.Services != null && model.Services.Count > 0)
        {
            foreach (var service in model.Services)
            {
                
                if (service.Tests != null && service.Tests.Count > 0)
                {
                    foreach (var test in service.Tests)
                    {
                        if (test.ServiceNotifications != null && test.ServiceNotifications.Count > 0)
                        {
                            foreach (var notification in test.ServiceNotifications)
                            {
                                notification.IsActive = false;
                            }
                        }
                        test.IsActive = false;

                    }
                }
                service.IsActive = false;
            }
        }
        model.IsActive = false;
        Update(model);
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        return await _context.Projects
                                .Include(P => P.ProjectOfficials)
                                    .Include(P => P.Services).ToListAsync();
    }

    public async Task<Project?> GetById(int id)
    {
        return await _context.Projects.AsQueryable().Where(P => P.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Project?> GetProjectByTitleAsync(string Title)
    {
        return await _context.Projects.Where(P => P.Title == Title).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Project?>> Search(DateTime? StartDate, DateTime? EndDate,string? Title)
    {
        DateTime start = StartDate ?? DateTime.MinValue;
        DateTime end = EndDate ?? DateTime.MaxValue;
        var Query = _context.Projects.AsQueryable().Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);

        if(Title != null) 
        {
            Query = Query.Where(x => x.Title == Title);
        }
        return await Query.Include(P => P.ProjectOfficials)
                                    .Include(P => P.Services).ToListAsync();
    }

    public void Update(Project model)
    {
        _context.Projects.Update(model);
    }
}
