
namespace notifier.Domain.Models;

[Table("ErrorLog", Schema = "dbo")]
public class ErrorLog
{
    public int ID {  get; set; }

    public string Description { get; set; } = null!;

}
