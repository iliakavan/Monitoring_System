namespace notifier.Application.Projects.Queries.GetById; 



public class GetProjectByIdQueryRequestValidator : AbstractValidator<GetProjectByIdQueryRequest>
{
    public GetProjectByIdQueryRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().GreaterThan(0).WithMessage("Given Id is Not Valid");
    }
}
