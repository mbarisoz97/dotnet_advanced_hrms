﻿using Ehrms.TrainingManagement.API.Handlers.Training.Queries;

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

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		var query = new GetTrainingByIdQuery { Id = id };
		var training = await _mediator.Send(query);
		var readTrainingDto = _mapper.Map<ReadTrainingDto>(training);

		return Ok(readTrainingDto);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var command = new DeleteTrainingCommand { Id = id };
		await _mediator.Send(command);
		
		return NoContent();
	}
}