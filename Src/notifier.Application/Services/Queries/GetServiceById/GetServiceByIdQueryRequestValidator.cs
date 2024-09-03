namespace notifier.Application.Services.Queries.GetServiceById;


public  class GetServiceByIdQueryRequestValidator : AbstractValidator<GetServiceByIdQueryRequest>
{
    public GetServiceByIdQueryRequestValidator()
    {
        RuleFor(S => S.Id).GreaterThan(0).WithMessage("Id is not valid");
    }
}
