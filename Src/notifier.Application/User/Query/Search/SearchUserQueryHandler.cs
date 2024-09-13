namespace notifier.Application.User.Query.Search;




public class SearchUserQueryHandler(IUnitsOfWorks uow) : IRequestHandler<SearchUserQueryRequest, ResultResponse<IEnumerable<UserDto>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<UserDto>>> Handle(SearchUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitsOfWorks.UserRepo.Search(request.startDate,request.endDate, request.FirstName, request.LastName,request.Email,request.PhoneNumber,request.Role,request.UserName);

        if (user is null || !user.Any()) 
        { 
            return new() { Success = false };
        }

        return new() { Success = true, Value = user };
    }
}
