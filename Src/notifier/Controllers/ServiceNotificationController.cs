namespace notifier.Controllers;





[Route("[controller]")]
[ApiController]
public class ServiceNotificationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]

    public async Task<IActionResult> GetServiceNotification([FromQuery] GetServiceNotificationByIdQueryRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);

    }

    [HttpGet]
    [Route("Get All Service Notification")]

    public async Task<IActionResult> GetAllServiceNotification() 
    {
        var result = await _mediator.Send(new GetAllServiceNotificationQueryRequest());
        if (!result.Success)
        {
            return NoContent();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("Search")]

    public async Task<IActionResult> Search([FromQuery] string? StartDate, [FromQuery] string? EndDate, [FromQuery] NotificationType? notifieType, [FromQuery] int? servicetestid, [FromQuery] int? serviceId,int? projectId)
    {
        var result = await _mediator.Send(new SearchServiceNotificationQueryRequest(StartDate, EndDate,notifieType,serviceId,servicetestid,projectId));

        if (!result.Success)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpPost]

    public async Task<IActionResult> AddServiceNotification([FromBody] AddServiceNotificationCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]

    public async Task<IActionResult> UpdateServiceNotification([FromBody] UpdateServiceNotificationCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{ID}")]

    public async Task<IActionResult> DeleteServiceNotification(int ID) 
    {
        var query = new DeleteServiceNotificationCommandRequest() { Id = ID };
        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }
}

