
namespace notifier.Application.ServiceTests.Command.UpdateServiceTest;


public class UpdateServiceTestCommandHandler(IUnitsOfWorks uow) : IRequestHandler<UpdateServiceTestCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitOfWorks = uow;
    public async Task<ResultResponse> Handle(UpdateServiceTestCommandRequest request, CancellationToken cancellationToken)
    {
        var serviceTest = await _unitOfWorks.ServiceTestRepo.GetById(request.Id);
 

        if (serviceTest is null) 
        {
            return new() { Success = false };
        }

        serviceTest.PriodTime = request.PriodTime ?? serviceTest.PriodTime;
        serviceTest.TestType = request.TestType ?? serviceTest.TestType;
        serviceTest.ServiceId = request.ServiceId ?? serviceTest.ServiceId;
        _unitOfWorks.ServiceTestRepo.Update(serviceTest);
        await _unitOfWorks.SaveChanges();

        return new() { Success = true };

    }
}
