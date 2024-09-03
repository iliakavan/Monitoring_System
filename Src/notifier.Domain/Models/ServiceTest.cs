namespace notifier.Domain.Models;

[Table("ServiceTests", Schema = "dbo")]
public class ServiceTest : BaseEntity
{

    public int PriodTime {  get; set; }

    public TestType TestType { get; set; }

    public int ServiceId {  get; set; }

    public Service Service { get; set; } = null!;

    public ICollection<ServiceNotfications> ServiceNotifications { get; } = new List<ServiceNotfications>();
}
