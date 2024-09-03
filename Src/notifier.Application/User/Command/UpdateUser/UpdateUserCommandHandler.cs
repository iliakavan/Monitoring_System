namespace notifier.Application.User.Command.UpdateUser;



public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks;

    public UpdateUserCommandHandler(IUnitsOfWorks uow)
    {
        _unitsOfWorks = uow;
    }
    public async Task<ResultResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitsOfWorks.UserRepo.GetUserByID(request.Id);

        if (user == null)
        {
            return new() { Success = false };
        }

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        _unitsOfWorks.UserRepo.Update(user);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true };
    }
}
