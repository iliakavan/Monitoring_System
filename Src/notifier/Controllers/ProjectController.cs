namespace notifier.Controllers;





[Route("[controller]")]
[ApiController]
public class ProjectController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{Id}")]

    public async Task<IActionResult> GetProjectById(int Id) 
    {
        var qurey = new GetProjectByIdQueryRequest() { Id = Id };

        var result = await _mediator.Send(qurey);

        if(!result.Success) 
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpPost]

    public async Task<IActionResult> AddProject([FromBody] AddProjectCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if(!result.Success) 
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectCommandRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }


    [HttpDelete("{ID}")]

    public async Task<IActionResult> DeleteProject(int ID) 
    {
        var qurey = new DeleteProjectCommandRequest() { Id = ID };

        var result = await _mediator.Send(qurey);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() 
    {
        var query = new GetAllProjectQueryRequest();

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);

    }

    [HttpGet]

    [Route("Search")]

    public async Task<IActionResult> Search([FromQuery] string? StartDate, [FromQuery] string? EndDate, [FromQuery] string? Title) 
    {
        var result = await _mediator.Send(new SearchProjectQueryRequest(EndDate, StartDate,Title));
        if (!result.Success)
        {
            return NoContent();
        }
        return Ok(result);

    }

}
