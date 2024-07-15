using Microsoft.AspNetCore.Authorization;
using Ehrms.ProjectManagement.API.Handlers.Project.Queries;
using Ehrms.ProjectManagement.API.Handlers.Employment.Queries;

namespace Ehrms.ProjectManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/Project/[controller]")]
public class EmploymentController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public EmploymentController(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	[HttpGet("{projectId}")]
	public async Task<IActionResult> GetProjectEmploymentHistory(Guid projectId)
	{
		GetEmploymentByProjectIdQuery query = new() { Id = projectId };
		var project = await _mediator.Send(query);

		var readProjectHistoryDto = _mapper.ProjectTo<EmploymentDto>(project);
		return Ok(readProjectHistoryDto);
	}
}
