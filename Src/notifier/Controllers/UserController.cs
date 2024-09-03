using notifier.Application.Authentication.Command.ChangePassword;
using notifier.Application.Authentication.Command.UserRegisteration;

namespace notifier.Controllers;


[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody]UserRegesterationCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success) 
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordCommandRequest request) 
    {
        var result = await _mediator.Send(request);

        if (!result.Success) 
        {
            return Unauthorized();
        }
        return Ok(result);
    }
}
