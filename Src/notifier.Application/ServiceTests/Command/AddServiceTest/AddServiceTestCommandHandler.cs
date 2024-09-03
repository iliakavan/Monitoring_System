namespace notifier.Application.ServiceTests.Command.AddServiceTest;





public class AddServiceTestCommandHandler(IUnitsOfWorks uow) : IRequestHandler<AddServiceTestCommandRequest,ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;

    public async Task<ResultResponse> Handle(AddServiceTestCommandRequest request, CancellationToken cancellationToken)
    {
        var service = await _unitsOfWorks.ServiceRepo.GetById(request.ServiceId);
        if(service is null)
        {
            return new() { Message = $"Service with Id of {request.ServiceId} does not exist." };
        }
        var ServiceTest = new ServiceTest()
        {
            ServiceId = request.ServiceId,
            PriodTime = request.PriodTime,
            TestType = request.TestType,
            RecordDate = DateTime.UtcNow,
        };

        await _unitsOfWorks.ServiceTestRepo.Add(ServiceTest);
        await _unitsOfWorks.SaveChanges();

        return new() { Success = true ,Message = "Test Added Successfully."};
    }
}
