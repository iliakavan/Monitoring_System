namespace notifier.Domain.BaseModel;


public class BaseEntity
{
    public int Id { get; set; }

    public DateTime RecordDate { get; set; }

    public bool IsActive { get; set; } = true;
}
