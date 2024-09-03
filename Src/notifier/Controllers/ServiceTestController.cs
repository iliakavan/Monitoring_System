﻿using notifier.Application.ServiceTests.Command.TestServices;

namespace notifier.Controllers;




[Route("[controller]")]
[ApiController]
public class ServiceTestController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet]

    public async Task<IActionResult> GetServiceTests() 
    {
        var result = await _mediator.Send(new GetAllServiceTestQueryRequest());

        if (!result.Success) 
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpGet]

    [Route("GetServiceTest")]

    public async Task<IActionResult> GetServiceTestById([FromQuery] GetServiceTestByIdQueryRequest request) 
    {
        var result = await _mediator.Send(request);
        
        if (!result.Success)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet]

    [Route("Test")]

    public async Task<IActionResult> Test()
    {
        var result = await _mediator.Send(new TestServicesCommand());

        if (!result.Success)
        {
            return NotFound();
        }
        return Ok(result);
    }


    [HttpGet]
    [Route("Search")]

    public async Task<IActionResult> Search([FromQuery] string? StartDate, [FromQuery] string? EndDate, [FromQuery] int? ServiceId, [FromQuery] int? ProjectId) 
    {
        var result = await _mediator.Send(new SearchServiceTestQueryRequest(StartDate, EndDate,ServiceId,ProjectId));

        if (!result.Success)
        {
            return NoContent();
        }
        return Ok(result);

    }


    [HttpPost]

    public async Task<IActionResult> AddServiceTest([FromBody] AddServiceTestCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]

    public async Task<IActionResult> UpdateServiceTest([FromBody] UpdateServiceTestCommandRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{ID}")]

    public async Task<IActionResult> DeleteServiceTest(int ID) 
    {
        var qurey = new DeleteServiceTestCommandRequest() { Id = ID };
        var result = await _mediator.Send(qurey);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result);

    }


}
