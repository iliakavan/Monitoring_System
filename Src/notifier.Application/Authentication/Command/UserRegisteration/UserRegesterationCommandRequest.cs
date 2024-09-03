namespace notifier.Application.Authentication.Command.UserRegisteration;



public class UserRegesterationCommandRequest : IRequest<ResultResponse>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber {  get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

}
