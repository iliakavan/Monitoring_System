namespace notifier.Application.Services.Commands.AddService;




public class AddServiceCommandHandler(IUnitsOfWorks uow) : IRequestHandler<AddServiceCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitsOfWorks = uow;
    public async Task<ResultResponse> Handle(AddServiceCommandRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new() { Success = false, Message = "request not valid" };
        }

        var service = new Service()
        {
            Title = request.Title,
            Url = request.Url,
            Ip = request.Ip,
            Port = request.Port,
            Method = request.Method,
            RecordDate = DateTime.Now,
            ProjectId = request.ProjectId
        };
        await _unitsOfWorks.ServiceRepo.Add(service);
        await _unitsOfWorks.SaveChanges();
        return new() { Success = true };
    }
}
