namespace notifier.Domain.Repositories;



public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetProjectByTitleAsync(string Title);
    Task<IEnumerable<Project?>> Search(DateTime? StartDate, DateTime? EndDate, string? Title);
    Task<Project?> GetByIdIncludeAll(int id);

}
