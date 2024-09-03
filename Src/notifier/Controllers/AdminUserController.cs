using Microsoft.AspNetCore.Authorization;
using notifier.Application.User.Command.DeactiveUser;
using notifier.Application.User.Query.Search;

namespace notifier.Controllers;



[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]

    public async Task<IActionResult> Search([FromQuery] SearchUserQueryRequest request) 
    {
        var result = await _mediator.Send(request);

        if(!result.Success) 
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{Id:int}")]

    public async Task<IActionResult> DeactiveUser(int Id) 
    {
        var result = await _mediator.Send(new DeactiveUserCommandRequest { Id = Id });

        if(!result.Success) 
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
