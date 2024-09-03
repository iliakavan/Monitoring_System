namespace notifier.Application.ServiceTests.Queries.GetServiceTestById;



public class GetServiceTestByIdQueryValidator : AbstractValidator<GetServiceTestByIdQueryRequest>
{
    public GetServiceTestByIdQueryValidator()
    {
        RuleFor(S => S.Id).GreaterThan(0).WithMessage("Given id is not valid");
    }
}
