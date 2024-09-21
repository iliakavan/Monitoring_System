

namespace notifier.Infrastructure.Repositories;

public class ErrorLogRepository(AppDbcontext context) : IErrorLogRepository
{
    private readonly AppDbcontext _context = context;
    public async Task Add(ErrorLog error)
    {
        await _context.Errors.AddAsync(error);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
