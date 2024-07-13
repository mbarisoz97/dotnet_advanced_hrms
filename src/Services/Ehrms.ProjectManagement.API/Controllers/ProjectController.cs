using Microsoft.AspNetCore.Authorization;
using Ehrms.ProjectManagement.API.Handlers.Project.Queries;

namespace Ehrms.ProjectManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProjectController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand createProjectcommand)
    {
        var project = await _mediator.Send(createProjectcommand);
        var readProjectDto = _mapper.Map<ReadProjectDto>(project);

        return Ok(readProjectDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteProjectCommand command = new() { Id = id };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        GetProjectsQuery query = new();
        var project = await _mediator.Send(query);
        var readProjectDtoCollection = _mapper.ProjectTo<ReadProjectDto>(project);

        return Ok(readProjectDtoCollection);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        GetProjectByIdQuery query = new() { Id = id };
        var project = await _mediator.Send(query);
        var readProjectDto = _mapper.Map<ReadProjectDto>(project);

        return Ok(readProjectDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateProjectCommand updateProjectcommand)
    {
        var project = await _mediator.Send(updateProjectcommand);
        var readProjectDto = _mapper.Map<ReadProjectDto>(project);

        return Ok(readProjectDto);
    }
}