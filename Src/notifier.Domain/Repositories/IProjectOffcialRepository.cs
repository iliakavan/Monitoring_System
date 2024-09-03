namespace notifier.Domain.Repositories;





public interface IProjectOffcialRepository : IRepository<ProjectOfficial>
{
    Task<IEnumerable<string>> FetchTelegramId(int projectId);
    Task<IEnumerable<ProjectOfficialDto>> Search(DateTime? StartDate, DateTime? EndDate,string? responsible,string? mobile,string? telegramID,int? projectid);
}
