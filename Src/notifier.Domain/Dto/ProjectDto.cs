namespace notifier.Domain.Dto;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public string? Description { get; set; }

    public DateTime RecordDate { get; set; }
}
