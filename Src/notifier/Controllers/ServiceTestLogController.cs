using Microsoft.AspNetCore.Authorization;
using notifier.Application.ServiceTestLogs.Query.SearchV1;
using notifier.Application.ServiceTestLogs.Query.SearchV2;

namespace notifier.Controllers;


[Route("[controller]")]
[ApiController]
public class ServiceTestLogController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    

    [HttpGet]
    [Authorize(Roles = "User,Admin, Manager")]

    public async Task<IActionResult> Search([FromQuery] string? Start, [FromQuery] string? End, [FromQuery] int? serviceId ) 
    {
        var result = await _mediator.Send(new SearchServiceTestLog(Start, End,serviceId));

        if (!result.Success) 
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpGet]
    [Route("Search")]
    [Authorize(Roles = "User,Admin, Manager")]

    public async Task<IActionResult> Search([FromQuery] string? Start, [FromQuery] string? End, [FromQuery] int? serviceId, [FromQuery] string? responseCode, [FromQuery] TestType? testtype, [FromQuery] int? projectId, [FromQuery] string? ip, [FromQuery] int? port, [FromQuery] string? url) 
    {
        var result = await _mediator.Send(new SearchServiceTestLogV2QueryRequest(Start,End,serviceId,responseCode,testtype,projectId,ip,port,url));
        
        if (!result.Success)
        {
            return NoContent();
        }
        return Ok(result);
    }

}
