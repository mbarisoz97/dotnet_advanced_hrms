using Ehrms.TrainingManagement.API.Handlers.Training.Queries;
using Microsoft.AspNetCore.Authorization;

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
}