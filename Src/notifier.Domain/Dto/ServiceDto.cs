namespace notifier.Domain.Dto;

public class ServiceDto
{
    public int Id { get; set; }

    public  string? Title { get; set; }
    
    public string? Url { get; set; }

    public string? Ip { get; set; }

    public int? Port { get; set; }

    public string? Method { get; set; }

    public DateTime RecordDate { get; set; }

    public int ProjectId { get; set; }

    public string? ProjectTitle { get; set; }

}
