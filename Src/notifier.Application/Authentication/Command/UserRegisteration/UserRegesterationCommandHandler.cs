namespace notifier.Application.Authentication.Command.UserRegisteration;




public class UserRegesterationCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UserRegesterationCommandRequest,ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse> Handle(UserRegesterationCommandRequest request, CancellationToken cancellationToken)
    {
        if(request is null) 
        {
            return new() { Success = false };
        }

        Users User = new() 
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            RecordDate = DateTime.Now,
            Password = BC.EnhancedHashPassword(request.Password)
        };

        await _unitsOfWorks.UserRepo.Register(User);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true };
    }

}