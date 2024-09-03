namespace notifier.Application.ProjectOfficials.Commands.UpdateProjectOffical;


public class UpdateProjectOfficialCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }
    public string? Responsible { get; set; }

    public string? Mobile { get; set; }

    public string? TelegramId { get; set; }

    public int? ProjectId { get; set; }
}
