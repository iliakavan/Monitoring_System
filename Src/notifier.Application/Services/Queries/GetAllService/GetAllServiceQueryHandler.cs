namespace notifier.Application.Services.Queries.GetAllService;






public class GetAllServiceQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetAllServiceQueryRequest, ResultResponse<IEnumerable<Service>>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<IEnumerable<Service>>> Handle(GetAllServiceQueryRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.ServiceRepo.GetAll();

        if (service is null || !service.Any())
        {
            return new() { Success = false, Message = "Failed to fetch" };
        }
        return new() { Success = true, Value = service };
    }
}
