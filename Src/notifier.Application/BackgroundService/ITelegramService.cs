namespace notifier.Application.BackgroundService;



public interface ITelegramService
{
    Task SendMessage(string TelegramID,string Text);

}
