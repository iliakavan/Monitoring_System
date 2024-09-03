namespace notifier.Application.ServiceTests.Queries.GetServiceTestById;



public class GetServiceTestByIdQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetServiceTestByIdQueryRequest, ResultResponse<ServiceTest>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<ServiceTest>> Handle(GetServiceTestByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var serviceTest = await _unitsOfWorks.ServiceTestRepo.GetById(request.Id);

        if (serviceTest is null) 
        {
            return new() { Success = false ,Message = "Service Test could not be found"};
        }

        return new() { Success = true , Value = serviceTest };
    }
}
