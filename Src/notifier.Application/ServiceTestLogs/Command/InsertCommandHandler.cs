namespace notifier.Application.SeriveTestLogs.Command;



public class InsertCommandHandler(IUnitsOfWorks uow) : IRequestHandler<InsertCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(InsertCommandRequest request, CancellationToken cancellationToken)
    {
        var serviceTestLog = new ServiceTestLog()
        {
            RecordDate = DateTime.Now,
            ResponseCode = request.ResponseCode,
            ServiceId = request.ServiceId
        };

        await _unitsOfWorks.ServiceTestLogRepo.Insert(serviceTestLog);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true };
    }
}
