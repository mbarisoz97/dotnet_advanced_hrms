using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Dto;
using Microsoft.AspNetCore.Authorization;
using Ehrms.Authentication.API.Handlers.User.Queries;

namespace Ehrms.Authentication.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            user => Ok(_mapper.Map<ReadUserDto>(user)),
            err => BadRequest());

        return actionResult;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var query = new GetUsersQuery();
        var users = await _mediator.Send(query);
        var readUserDtos = _mapper.ProjectTo<ReadUserDto>(users);

        return Ok(readUserDtos);
    }
}