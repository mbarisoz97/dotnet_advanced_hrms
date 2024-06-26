using Ehrms.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Ehrms.Authentication.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ITokenHandler _tokenHandler;

    public AccountController(ITokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    [HttpPost]
    public IActionResult Authenticate([FromBody] AuthenticationRequest request)
    {
        var authenticationResponse = _tokenHandler.Generate(request);

        if (authenticationResponse == null)
        {
            return Unauthorized();
        }

        return Ok(authenticationResponse);
    }
}