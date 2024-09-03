
namespace notifier.Application.Authentication.Command.ChangePassword;



public class ChangePasswordCommandHandler(IUnitsOfWorks uow) : IRequestHandler<ChangePasswordCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitsOfWorks.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword);

        if(user is null) 
        {
            return new() { Success = false };
        }

        user.Password = request.newPassword;
        _unitsOfWorks.UserRepo.Update(user);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true };
    }
}
