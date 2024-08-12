using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Handlers.User.Commands;
using Microsoft.AspNetCore.Authorization;

namespace Ehrms.Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        var actionRes = result.Match<IActionResult>(
            m => Ok(),
            err => BadRequest());

        return actionRes;
    }
}