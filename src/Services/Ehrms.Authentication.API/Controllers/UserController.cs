using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Dto.User;
using Ehrms.Authentication.API.Extension;
using Microsoft.AspNetCore.Authorization;
using Ehrms.Authentication.API.Handlers.User.Queries;

namespace Ehrms.Authentication.API.Controllers;

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

    [Authorize(Roles = "Admin")]
    [HttpPut("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        var actionRes = result.Match<IActionResult>(
            user => Ok(_mapper.Map<RegisterUserResponseDto>(user)),
            err => BadRequest());

        return actionRes;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            Succ: user => Ok(_mapper.Map<UserUpdateResponseDto>(user)),
            Fail: this.MapUserUpdateFailureResult);

        return actionResult;
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("Reset")]
    public async Task<IActionResult> ResetPassword([FromBody] UpdateUserPasswordCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            Succ: user => Ok(_mapper.Map<UserUpdateResponseDto>(user)),
            Fail: this.MapUserResetPasswordFailureResult);

        return actionResult;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var query = new GetUsersQuery();
        var users = await _mediator.Send(query);
        var readUserDtos = _mapper.ProjectTo<ReadUserDto>(users);

        return Ok(readUserDtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery() { Id = id };
        var queryResult = await _mediator.Send(query);

        var actionResult = queryResult.Match<IActionResult>(
            user => Ok(_mapper.Map<ReadUserDto>(user)),
            err => BadRequest());

        return actionResult;
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("GetByName/{username}")]
    public async Task<IActionResult> GetUserByName(string username)
    {
        var query = new GetUserByNameQuery() { Username = username };
        var queryResult = await _mediator.Send(query);
        var actionResult = queryResult.Match<IActionResult>(
            user => Ok(_mapper.Map<ReadUserDto>(user)),
            err => BadRequest(err.Message));

        return actionResult;
    }
}