using Telegram.Bot;
using Telegram.Bot.Exceptions;


namespace notifier.Infrastructure.BackgroundServices;


public class TelegramService : ITelegramService
{

    private readonly TelegramBotClient _botClient;
    private readonly IUnitsOfWorks _uow;

    // Original constructor for production use

    public TelegramService(
        IUnitsOfWorks uow,
        IConfiguration config
        )
    {
        var botclient = config["TelegramBotToken"];
        _botClient = new TelegramBotClient(botclient!);
        _uow = uow;

    }

    public async Task SendMessage(string TelegramID, string Text)
    {
        try 
        {
            var send = await _botClient.SendTextMessageAsync(TelegramID, Text, disableWebPagePreview: true);

            if (send != null)
            {
                var message = new TelegramMassageLog()
                {
                    TelegramID = send.Chat.Username!,
                    Text = send.Text!,
                    RecordDate = DateTime.Now,
                    TimeSent = send.Date
                };
                await _uow.TelegramMassageLogRepo.Add(message);
            }
        }catch (ApiRequestException ex) 
        {
            var error = new ErrorLog()
            {
                Description = ex.Message
            };

            await _uow.ErrorLogRepo.Add(error);
        }catch(RequestException ex) 
        {
            var error = new ErrorLog()
            {
                Description = ex.Message
            };

            await _uow.ErrorLogRepo.Add(error);
        }
        
    }
}
