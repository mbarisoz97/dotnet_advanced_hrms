using Microsoft.AspNetCore.Authorization;
using Ehrms.TrainingManagement.API.Handlers.Training.Queries;
using Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;
using Ehrms.TrainingManagement.API.Handlers.Recommendation.Queries;
using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TrainingRecommendationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TrainingRecommendationController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpDelete("Recommendation/{id:guid}")]
    public async Task<IActionResult> DeleteRecommendationRequest(Guid id)
    {
        var command = new DeleteTrainingRecommendationRequestCommand { Id = id };
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("Recommendation/{id:guid}")]
    public async Task<IActionResult> GetRecommendationResult(Guid id)
    {
        var query = new GetTrainingRecommendationRequestByIdQuery { Id = id };
        var trainingRecommendationRequest = await _mediator.Send(query);
        var trainingRecommendationDto = _mapper.Map<ReadTrainingRequestDto>(trainingRecommendationRequest);

        return Ok(trainingRecommendationDto);
    }

    [HttpPost("Recommendation")]
    public async Task<IActionResult> RecommendTraining([FromBody] CreateTrainingRecommendationRequestCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("RecommendationRequests")]
    public async Task<IActionResult> GetRecommendationRequests()
    {
        var query = new GetTrainingRecommendationsQuery();
        var requestCollection = await _mediator.Send(query);
        var requestDtoCollection = _mapper.ProjectTo<ReadTrainingRequestDto>(requestCollection);

        return Ok(requestDtoCollection);
    }

    [HttpGet("RecommendationResult/{id:guid}")]
    public async Task<IActionResult> GetRecommendationRequest(Guid id)
    {
        var query = new GetTrainingRecommendationResultByIdQuery() { Id = id };
        var recommendationResult = await _mediator.Send(query);
        var recommendationResultDto = _mapper.Map<ReadTrainingRecommendationResultDto>(recommendationResult);

        return Ok(recommendationResultDto);
    }

    [HttpGet("RecommendationResults/{id:guid}")]
    public async Task<IActionResult> GetRecommendationRequests(Guid id)
    {
        var query = new GetTrainingRecommendationResultsQuery() { RecommendationRequestId = id };
        var recommendationResult = await _mediator.Send(query);
        var recommendationResultDto = _mapper.ProjectTo<ReadTrainingRecommendationResultDto>(recommendationResult);

        return Ok(recommendationResultDto);
    }

    [HttpPut("RecommendationPreferences")]
    public async Task<IActionResult> SetTrainingRecommendationPreferences(CreateTrainingRecommendationPreferenceCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            Succ: s => Ok(_mapper.Map<ReadRecommendationPreferenceDto>(s)),
            Fail: f => BadRequest(f.Message));

        return actionResult;
    }

    [HttpPost("RecommendationPreferences")]
    public async Task<IActionResult> UpdateTrainingRecommendationPreferences(UpdateTrainingRecommendationPreferenceCommand command)
    {
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            Succ: s => NoContent(),
            Fail: f => BadRequest(f.Message));

        return actionResult;
    }

    [HttpDelete("RecommendationPreferences/{id:guid}")]
    public async Task<IActionResult> DeleteTrainingRecommendationPreferences(Guid id)
    {
        var command = new DeleteTrainingRecommendationPreferenceCommand() { Id = id };
        var commandResult = await _mediator.Send(command);
        var actionResult = commandResult.Match<IActionResult>(
            Succ: s => NoContent(),
            Fail: f => BadRequest(f.Message));

        return actionResult;
    }

    [HttpGet("RecommendationPreferences")]
    public async Task<IActionResult> GetTrainingRecommendationPreferences()
    {
        var query = new GetRecommendationPreferencesQuery();
        var preferences = await _mediator.Send(query);
        var preferenceDtos = _mapper.ProjectTo<ReadRecommendationPreferenceDto>(preferences);

        return Ok(preferenceDtos);
    }

    [HttpGet("RecommendationPreferences/{id:guid}")]
    public async Task<IActionResult> GetTrainingRecommendationPreferences(Guid id)
    {
        var query = new GetRecommendationPreferencesByIdQuery() { Id = id};
        var queryResult = await _mediator.Send(query);
        var actionResult = queryResult.Match<IActionResult>(
            Succ: s => Ok(_mapper.Map<ReadRecommendationPreferenceDto>(s)),
            Fail: f => BadRequest(f.Message));

        return actionResult;
    }
}