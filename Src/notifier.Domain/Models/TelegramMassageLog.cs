namespace notifier.Domain.Models;


[Table("TelegramMassageLog", Schema = "dbo")]

public class TelegramMassageLog
{
    public int ID { get; set; }

    public string TelegramID { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public DateTime TimeSent { get; set; }
}
