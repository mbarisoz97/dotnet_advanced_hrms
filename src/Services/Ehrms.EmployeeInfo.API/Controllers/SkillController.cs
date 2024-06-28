using Microsoft.AspNetCore.Authorization;

namespace Ehrms.EmployeeInfo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SkillController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public SkillController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> Create([FromBody] CreateSkillCommand createSkillcommand)
    {
        var skill = await _mediator.Send(createSkillcommand);
        var readSkillDto = _mapper.Map<ReadSkillDto>(skill);

        return Ok(readSkillDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new GetSkillByIdQuery() { Id = id };
        var skill = await _mediator.Send(query);
        var readSkillDto = _mapper.Map<ReadSkillDto>(skill);

        return Ok(readSkillDto);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetSkillsQuery();
        var skillsCollection = await _mediator.Send(query);
        var readSkillDtos = _mapper.ProjectTo<ReadSkillDto>(skillsCollection);

        return Ok(readSkillDtos);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateSkillCommand updateSkillCommand)
    {
        var skill = await _mediator.Send(updateSkillCommand);
        var readSkillDto = _mapper.Map<ReadSkillDto>(skill);

        return Ok(readSkillDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteSkillCommand() { Id = id };
        await _mediator.Send(command);

        return NoContent();
    }
}