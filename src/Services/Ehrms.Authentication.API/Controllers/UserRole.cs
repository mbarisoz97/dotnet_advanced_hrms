using Microsoft.AspNetCore.Mvc;
using Ehrms.Authentication.API.Dto.Role;
using Microsoft.AspNetCore.Authorization;
using Ehrms.Authentication.API.Handlers.Role.Queries;

namespace Ehrms.Authentication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserRole : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserRole(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserRoles()
    {
        var query = new GetUserRolesQuery();
        var userRoles = await _mediator.Send(query);
        var userRoleDtos = _mapper.ProjectTo<ReadUserRoleDto>(userRoles);

        return Ok(userRoleDtos);
    }
}