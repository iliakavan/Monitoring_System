
namespace notifier.Application.User.Command.DeactiveUser;



public class DeactiveUserCommandHandler : IRequestHandler<DeactiveUserCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    public DeactiveUserCommandHandler(IUnitsOfWorks uow)
    {
        _unitsOfWorks = uow;
    }
    public async Task<ResultResponse> Handle(DeactiveUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitsOfWorks.UserRepo.GetUserByID(request.Id);

        if(user is null)
        {
            return new() { Success = false };
        }
        _unitsOfWorks.UserRepo.DeactiveUser(user);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true };
    }
}
