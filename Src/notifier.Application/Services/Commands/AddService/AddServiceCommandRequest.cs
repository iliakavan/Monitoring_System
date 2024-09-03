namespace notifier.Application.Services.Commands.AddService;



public class AddServiceCommandRequest : IRequest<ResultResponse>
{
    public required string Url { get; set; }

    public required string Title { get; set; }

    public required string Ip { get; set; }

    public required int Port { get; set; }

    public required string Method { get; set; }

    public int ProjectId { get; set; }
}
