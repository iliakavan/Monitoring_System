namespace notifier.Application.Authentication.Command.ChangePassword;



public class ChangePasswordCommandRequest : IRequest<ResultResponse>
{
    public string UsernameOrEmail { get; set; } = null!;

    public string CurrentPassword { get; set; } = null!;

    public string newPassword {  get; set; } = null!;
}
