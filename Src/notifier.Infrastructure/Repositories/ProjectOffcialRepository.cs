namespace notifier.Infrastructure.Repositories;




public class ProjectOffcialRepository(AppDbcontext context) : IProjectOffcialRepository
{
    private readonly AppDbcontext _context = context;

    public async Task Add(ProjectOfficial model)
    {
        await _context.ProjectOfficials.AddAsync(model);
    }

    public void Delete(ProjectOfficial model)
    {
        model.IsActive = false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<IEnumerable<string>> FetchTelegramId(int projectId)
    {
        return await _context.ProjectOfficials.Where(p => p.ProjectId == projectId).Select(x => x.TelegramId).ToListAsync();
    }

    public async Task<IEnumerable<ProjectOfficial>> GetAll()
    {
        return await _context.ProjectOfficials.ToListAsync();
    }

    public async Task<ProjectOfficial?> GetById(int id)
    {
        return await _context.ProjectOfficials.Where(P => P.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectOfficialDto>> Search(DateTime? StartDate, DateTime? EndDate, string? responsible, string? mobile, string? telegramID,int? projectId)
    {
        DateTime start = StartDate ?? DateTime.MinValue;
        DateTime end = EndDate ?? DateTime.MaxValue;
        // Apply date range filter
        var query = _context.ProjectOfficials.AsQueryable().Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);

        // Apply additional filters only if parameters are provided
        if (!string.IsNullOrEmpty(responsible))
        {
            query = query.Where(x => x.Responsible == responsible);
        }

        if (!string.IsNullOrEmpty(mobile))
        {
            query = query.Where(x => x.Mobile == mobile);
        }

        if (!string.IsNullOrEmpty(telegramID))
        {
            query = query.Where(x => x.TelegramId == telegramID);
        }
        if(projectId is not null) 
        {
            query = query.Where(x => x.ProjectId == projectId);
        }

        return await query.Select(po => new ProjectOfficialDto 
        {
            ID = po.Id,
            Mobile = po.Mobile,
            ProjectId = po.ProjectId,
            Responsible = po.Responsible,
            TelegramId = po.TelegramId,
            ProjectName = po.Project.Title,
            RecordDate = po.RecordDate
        }).ToListAsync();
    }

    public void Update(ProjectOfficial model)
    {
        _context.ProjectOfficials.Update(model);
    }
}
