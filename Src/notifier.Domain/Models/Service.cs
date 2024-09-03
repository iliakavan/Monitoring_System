

namespace notifier.Domain.Models;


[Table("Services", Schema = "dbo")]
public class Service : BaseEntity
{
    public string? Url { get; set; }

    public required string Title { get; set; }

    public string? Ip { get; set; }

    public int? Port { get; set; } = null;

    public string? Method { get; set; }

    public int ProjectId {  get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<ServiceTest> Tests { get; } = new List<ServiceTest>();
    public ICollection<ServiceTestLog> TestsLog { get; } = new List<ServiceTestLog>();
}
