using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Extension;
using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserCommand command)
    {
        var result = await _mediator.Send(command);
        var actionResult = result.Match(
            Succ: Ok,
            Fail: this.MapLoginFailureResult);

        return actionResult;
    }

    [HttpPost("Refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshAuthenticationCommand command)
    {
        var result = await _mediator.Send(command);
        var actionResult = result.Match(
            Succ: Ok,
            Fail: this.MapRefreshFailureResult);

        return actionResult;
    }
}