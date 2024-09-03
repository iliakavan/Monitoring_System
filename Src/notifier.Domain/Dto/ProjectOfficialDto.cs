namespace notifier.Domain.Dto;



public class ProjectOfficialDto
{
    public int ID { get; set; }
    public required string Responsible { get; set; }

    public required string Mobile { get; set; }

    public required string TelegramId { get; set; }

    public DateTime RecordDate {  get; set; }

    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }
}
