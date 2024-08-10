using Ehrms.Shared;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Ehrms.Authentication.API.Models;
using Ehrms.Authentication.API.Database.Models;
using Ehrms.Authentication.API.Extension;

namespace Ehrms.Authentication.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly ITokenHandler _tokenHandler;
	private readonly UserManager<User> _userManager;

	public AccountController(ITokenHandler tokenHandler, UserManager<User> userManager)
	{
		_tokenHandler = tokenHandler;
		_userManager = userManager;
	}

	[HttpPost("Login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
	{
		var user = await _userManager.FindByNameAsync(request.Username);
		if (user == null)
		{
			return Unauthorized();
		}

		if (!user.IsActive)
		{
			return Unauthorized();
		}

		if (!await _userManager.CheckPasswordAsync(user, request.Password))
		{
			return Unauthorized();
		}

		var authenticationResponse = _tokenHandler.Generate(request);
		if (authenticationResponse == null)
		{
			return Unauthorized();
		}

		user.RefreshToken = GenerateRefreshToken();
		user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(10);

		await _userManager.UpdateAsync(user);
		authenticationResponse.Username = request.Username;
		authenticationResponse.RefreshToken = user.RefreshToken;

		return Ok(authenticationResponse);
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

		var user = await _userManager.FindByNameAsync(userName);
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