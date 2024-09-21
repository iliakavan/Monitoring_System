

namespace notifier.Domain.Repositories;

public interface IErrorLogRepository : IDisposable
{
    Task Add(ErrorLog error);

}
