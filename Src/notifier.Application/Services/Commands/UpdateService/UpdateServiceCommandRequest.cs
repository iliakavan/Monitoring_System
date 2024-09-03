namespace notifier.Application.Services.Commands.UpdateService;


public class UpdateServiceCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }
    public string? Url { get; set; }

    public  string? Title { get; set; }

    public  string? Ip { get; set; }

    public int? Port { get; set; }

    public  string? Method { get; set; }

    public int? ProjectId { get; set; }
}
