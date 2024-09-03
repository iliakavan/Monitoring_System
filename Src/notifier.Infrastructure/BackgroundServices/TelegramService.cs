using Telegram.Bot;


namespace notifier.Infrastructure.BackgroundServices;


public class TelegramService : ITelegramService
{
    private readonly TelegramBotClient _botClient = new("7236806019:AAGw_ODE5EGUIlQT8dMfMxsjtkDTod3MJck");

    public async Task SendMessage(string TelegramID, string Text)
    {
        await _botClient.SendTextMessageAsync(TelegramID, Text,disableWebPagePreview:true);
    }
}
