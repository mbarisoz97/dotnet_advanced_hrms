using Ehrms.Shared;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserManagerAdapter _userManagerWrapper;

	public AccountController(IMediator mediator, ITokenHandler tokenHandler, IUserManagerAdapter userManagerAdapter)
	{
        _mediator = mediator;
        _tokenHandler = tokenHandler;
        _userManagerWrapper = userManagerAdapter;
	}

	[HttpPost("Login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserCommand command)
	{
        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(
            success => Ok(success),
            failure => Unauthorized(failure.Message)
        );
    }

	[HttpPost("Refresh")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Refresh([FromBody] RefreshAuthenticationCommand command)
	{
		var result = await _mediator.Send(command);

		return result.Match<IActionResult>(
			accessToken => Ok(accessToken),
			exception => Unauthorized(exception.Message));
    }
}