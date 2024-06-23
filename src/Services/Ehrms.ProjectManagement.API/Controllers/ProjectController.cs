using Ehrms.ProjectManagement.API.Handlers.Project.Queries;

namespace Ehrms.ProjectManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        var command = _mapper.Map<CreateProjectCommand>(createProjectDto);
        var project = await _mediator.Send(command);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        GetProjectByIdQuery query = new() { Id = id };
        var project = await _mediator.Send(query);
        var readProjectDto = _mapper.Map<ReadProjectDto>(project);

        return Ok(readProjectDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateProjectDto updateProjectDto)
    {
        var command = _mapper.Map<UpdateProjectCommand>(updateProjectDto);
        var project = await _mediator.Send(command);
        var readProjectDto = _mapper.Map<ReadProjectDto>(project);

        return Ok(readProjectDto);
    }
}