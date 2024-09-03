namespace notifier.Application.Services.Queries.GetServiceById;



public class GetServiceByIdQueryRequest : IRequest<ResultResponse<ServiceDto>>
{
    public int Id { get; set; }
}
