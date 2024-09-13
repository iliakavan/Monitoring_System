namespace notifier.Application.Services.Commands.DeleteService;



public class DeleteServiceCommandHandler(IUnitsOfWorks uow) : IRequestHandler<DeleteServiceCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(DeleteServiceCommandRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.ServiceRepo.GetByIdIncludeAll(request.Id);
        
        if (service is null)
        {
            return new() { Message = "Service doesn't exist.", Success = false };
        }

        _unitsOfWorks.ServiceRepo.Delete(service);
        await _unitsOfWorks.SaveChanges();
        return new ResultResponse() { Success = true, Message = $"{service.Title} Deleted Successfully" };
    }
}
