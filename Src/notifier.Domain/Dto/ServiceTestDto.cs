namespace notifier.Domain.Dto;

public class ServiceTestDto
{
    public int Id { get; set; }
    public int PriodTime { get; set; }
    public string TestType { get; set; } = null!;
    public int ServiceId { get; set; }
    public DateTime RecordDate { get; set; }
    public string? ServiceName { get; set; }
    public string? ProjectName {  get; set; }
    public int ProjectID {  get; set; }
}
