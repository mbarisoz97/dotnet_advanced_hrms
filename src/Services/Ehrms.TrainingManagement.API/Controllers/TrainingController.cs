namespace Ehrms.TrainingManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrainingController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public TrainingController(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	[HttpPut]
	public async Task<IActionResult> Create([FromBody] CreateTrainingDto createTrainingDto)
	{
		var command = _mapper.Map<CreateTrainingCommand>(createTrainingDto);
		var training = await _mediator.Send(command);
		var readTraniningDto = _mapper.Map<ReadTrainingDto>(training);

		return Ok(readTraniningDto);
	}
}