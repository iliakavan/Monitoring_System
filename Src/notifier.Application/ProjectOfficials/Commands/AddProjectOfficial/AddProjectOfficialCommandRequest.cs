namespace notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;


public class AddProjectOfficialCommandRequest : IRequest<ResultResponse>
{
    public required string Responsible { get; set; }

    public required string Mobile { get; set; }

    public required string TelegramId { get; set; }

    public int ProjectId { get; set; }
}
