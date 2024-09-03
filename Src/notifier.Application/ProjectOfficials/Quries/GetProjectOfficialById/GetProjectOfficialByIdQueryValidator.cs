namespace notifier.Application.ProjectOfficials.Quries.GetProjectOfficialById;



public class GetProjectOfficialByIdQueryValidator : AbstractValidator<GetProjectOfficialByIdQueryRequest>
{
    public GetProjectOfficialByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull().GreaterThan(0).WithMessage("Given Id is Not Valid");
    }
}
