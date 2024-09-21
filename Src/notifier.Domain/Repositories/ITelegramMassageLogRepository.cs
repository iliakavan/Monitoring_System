namespace notifier.Domain.Repositories;


public interface ITelegramMassageLogRepository : IDisposable
{
    Task Add(TelegramMassageLog model);

}
