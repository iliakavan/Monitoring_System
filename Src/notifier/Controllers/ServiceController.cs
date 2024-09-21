using Microsoft.AspNetCore.Authorization;

namespace notifier.Controllers;













[Route("[controller]")]
[ApiController]
public class ServiceController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "User,Admin,Manager")]

    public async Task<IActionResult> GetServiceById([FromQuery] GetServiceByIdQueryRequest request) 
    {
        var result = await _mediator.Send(request);

        if(!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("List Of Services")]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> GetAllService() 
    {
        var result = await _mediator.Send(new GetAllServiceQueryRequest());

        if(!result.Success) 
        {
            return NoContent();
        }
        
        return Ok(result);
    }

    [HttpGet]
    [Route("Search")]
    [Authorize(Roles = "User,Admin,Manager")]
    public async Task<IActionResult> SearchService([FromQuery] string? StartDate, [FromQuery] string? EndDate, [FromQuery] string? url,[FromQuery] string? title, [FromQuery] string? ip, [FromQuery] int? port, [FromQuery] int? projectid) 
    {
        var result = await _mediator.Send(new SearchServiceQueryRequest() { EndDate = EndDate,StartDate = StartDate, Title = title,Ip = ip,Port = port,Url = url, ProjectId = projectid });

        if(!result.Success) 
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> AddService([FromBody] AddServiceCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if(!result.Success) 
        {
            return BadRequest();
        }
        return Ok(result);

    }


    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> UpdateService([FromBody] UpdateServiceCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{ID}")]
    [Authorize(Roles = "Admin, Manager")]


    public async Task<IActionResult> DeleteService(int ID)
    {
        var qurey = new DeleteServiceCommandRequest { Id = ID };

        var result = await _mediator.Send(qurey);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }



}


