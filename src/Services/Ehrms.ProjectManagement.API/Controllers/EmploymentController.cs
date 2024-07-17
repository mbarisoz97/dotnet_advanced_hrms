using Microsoft.AspNetCore.Authorization;
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
		var employments = await _mediator.Send(query);

		var readProjectHistoryDto = _mapper.ProjectTo<ProjectEmploymentDto>(employments);
		return Ok(readProjectHistoryDto);
	}

	[HttpGet("Employee/{employeeId:guid}")]
	public async Task<IActionResult> GetEmployeeProjectHistory(Guid employeeId)
	{
		GetEmploymentByEmployeeIdQuery query = new() { Id = employeeId };
		var employments = await _mediator.Send(query);
		
		var readProjectHistoryDto = _mapper.ProjectTo<WorkerEmploymentDto>(employments);
		return Ok(readProjectHistoryDto);
	}
}