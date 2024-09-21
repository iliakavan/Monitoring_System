
namespace notifier.Infrastructure.Repositories;


public class TelegramMassageLogRepository(AppDbcontext context) : ITelegramMassageLogRepository
{
    private readonly AppDbcontext _context = context;
    public async Task Add(TelegramMassageLog model)
    {
        await _context.AddAsync(model);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
