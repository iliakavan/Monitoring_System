using System.ComponentModel.DataAnnotations.Schema;

namespace notifier.Domain.Models;

[Table("Projects",Schema = "dbo")]
public class Project : BaseEntity
{
    public required string Title {  get; set; }

    public  string? Description { get; set; }

    public ICollection<ProjectOfficial> ProjectOfficials { get; } = new List<ProjectOfficial>();

    public ICollection<Service> Services { get; } = new List<Service>();
}
