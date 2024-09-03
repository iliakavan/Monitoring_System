namespace notifier.Application.ServiceTests.Queries.GetAllServiceTest;




public class GetAllServiceTestQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetAllServiceTestQueryRequest, ResultResponse<IEnumerable<ServiceTest>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<ServiceTest>>> Handle(GetAllServiceTestQueryRequest request, CancellationToken cancellationToken)
    {
        var ServiceTest = await _unitsOfWorks.ServiceTestRepo.GetAll();

        if(!ServiceTest.Any() || ServiceTest is null) 
        { 
            return new() { Success = false,Message = "ServiceTest is Empty Or Null" };
        }

        return new() { Success = true, Value = ServiceTest };
    }
}
