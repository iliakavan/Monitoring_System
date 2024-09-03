using System.ComponentModel.DataAnnotations.Schema;

namespace notifier.Domain.Models;




[Table("ProjectOfficials", Schema = "dbo")]
public class ProjectOfficial : BaseEntity
{
    public required string Responsible {  get; set; }

    public required string Mobile {  get; set; }

    public required string TelegramId {  get; set; }

    public int ProjectId {  get; set; }
    public Project Project { get; set; } = null!;
}
