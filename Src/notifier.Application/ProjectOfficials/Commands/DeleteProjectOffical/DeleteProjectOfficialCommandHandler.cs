namespace notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;




public class DeleteProjectOfficialCommandHandler(IUnitsOfWorks uow) : IRequestHandler<DeleteProjectOfficialCommandRequest, ResultResponse>
{
    private readonly IUnitsOfWorks _unitOfWorks = uow;

    public async Task<ResultResponse> Handle(DeleteProjectOfficialCommandRequest request, CancellationToken cancellationToken)
    {
        var projectOfficial = await _unitOfWorks.ProjectOffcialRepo.GetById(request.Id);

        if (projectOfficial is null) 
        {
            return new() { Success = false };
        }
        _unitOfWorks.ProjectOffcialRepo.Delete(projectOfficial);
        await _unitOfWorks.SaveChanges();

        return new() { Success = true };
    }
}
