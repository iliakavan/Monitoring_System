namespace notifier.Application.User.Command.DeactiveUser;



public class DeactiveUserCommandRequest : IRequest<ResultResponse>
{
    public int Id { get; set; }

}
