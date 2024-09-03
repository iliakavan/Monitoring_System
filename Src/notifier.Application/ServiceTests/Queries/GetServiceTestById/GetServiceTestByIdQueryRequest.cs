namespace notifier.Application.ServiceTests.Queries.GetServiceTestById;



public class GetServiceTestByIdQueryRequest : IRequest<ResultResponse<ServiceTest>>
{
    public int Id { get; set; }

}
