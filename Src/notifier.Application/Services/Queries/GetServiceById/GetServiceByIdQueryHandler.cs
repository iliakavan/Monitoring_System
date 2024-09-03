namespace notifier.Application.Services.Queries.GetServiceById; 






public class GetServiceByIdQueryHandler(IUnitsOfWorks uow) : IRequestHandler<GetServiceByIdQueryRequest, ResultResponse<ServiceDto>>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse<ServiceDto>> Handle(GetServiceByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.ServiceRepo.GetById(request.Id);

        if (service is null)
        {
            return new() { Message = "Service doesn't exist.", Success = false };
        }

        var ServiceDto = new ServiceDto()
        {
            Title = service.Title,
            Url = service.Url,
            Ip = service.Ip,
            Port = service.Port.Value,
            Method = service.Method
        };

        return new() { Success = true, Value = ServiceDto };
    }
}
