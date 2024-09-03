
namespace notifier.Application.Services.Commands.UpdateService;



public class UpdateServiceCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UpdateServiceCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(UpdateServiceCommandRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.ServiceRepo.GetById(request.Id);

        if (service is null)
        {
            return new() { Success = false, Message = "Service doesnt Exist" };
        }

        service.Title = request.Title ?? service.Title;
        service.Url = request.Url ?? service.Url;
        service.Method = request.Method ?? service.Method;
        service.Port = request.Port ?? service.Port;
        service.Ip = request.Ip ?? service.Ip;
        service.ProjectId = request.ProjectId ?? service.ProjectId;
        _unitsOfWorks.ServiceRepo.Update(service);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true, Message = $"{request.Id} Updated Successfully" };
    }
}
