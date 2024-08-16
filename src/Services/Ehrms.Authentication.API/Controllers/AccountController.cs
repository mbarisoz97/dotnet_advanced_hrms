using Ehrms.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Ehrms.Authentication.API.Models;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Extension;
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
	public async Task<IActionResult> Refresh([FromBody] RefreshModel refreshTokenRequest)
	{
		var principal = _tokenHandler.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
		var userName = principal?.Identity?.Name ?? string.Empty;
		if (userName.IsNullOrEmpty())
		{
			return Unauthorized();
		}

		var user = await _userManagerWrapper.FindByNameAsync(userName);
		if (!refreshTokenRequest.HasValidRefreshToken(user!))
		{
			return Unauthorized();
		}

		var newToken = _tokenHandler.Generate(new AuthenticationRequest
		{
			Username = userName
		});

		if (newToken != null)
		{
			newToken.RefreshToken = refreshTokenRequest.RefreshToken;
		}

		return Ok(newToken);
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var generator = RandomNumberGenerator.Create();
		generator.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}
}