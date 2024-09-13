using Microsoft.AspNetCore.Authorization;

namespace notifier.Controllers;




[Route("[controller]")]
[ApiController]
public class ProjectOfficialController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> GetProjectOfficials() 
    {
        var result = await _mediator.Send(new GetAllProjectOfficialQueryRequest());

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("ById")]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> GetProjectOfficialById([FromQuery] GetProjectOfficialByIdQueryRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);

    }

    [HttpGet]
    [Route("Search")]
    [Authorize(Roles = "User,Admin,Manager")]

    public async Task<IActionResult> Search([FromQuery] string? StartDate, [FromQuery] string? EndDate, [FromQuery] string? Responsible, [FromQuery] string? Mobile, [FromQuery] string? TelegramId, [FromQuery] int? ProjectID) 
    {
        var result = await _mediator.Send(new SearchProjectOfficialQueryRequest(StartDate, EndDate,Responsible,Mobile,TelegramId,ProjectID));

        if (!result.Success)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> AddProjectOfficial([FromBody] AddProjectOfficialCommandRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> UpdateProjectOfficial([FromBody] UpdateProjectOfficialCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{ID}")]
    [Authorize(Roles = "Admin,Manager")]

    public async Task<IActionResult> DeleteProjectOfficial(int ID)
    {
        var qurey = new DeleteProjectOfficialCommandRequest() { Id = ID };

        var result = await _mediator.Send(qurey);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }

}
