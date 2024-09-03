namespace notifier.Application.ServiceTests.Command.DeleteServiceTest;




public class DeleteServiceTestCommandHandler(IUnitsOfWorks uow) : IRequestHandler<DeleteServiceTestCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(DeleteServiceTestCommandRequest request, CancellationToken cancellationToken)
    {
        var ServiceTest = await _unitsOfWorks.ServiceTestRepo.GetById(request.Id);
        
        if(ServiceTest is null) 
        {
            return new() { Message = "Service Test Doesnt Exist !!", Success = false };
        }

        _unitsOfWorks.ServiceTestRepo.Delete(ServiceTest);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true };
    }
}
