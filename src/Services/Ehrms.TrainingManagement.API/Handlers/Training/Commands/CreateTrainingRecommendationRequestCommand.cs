namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;

public sealed class CreateTrainingRecommendationRequestCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}

internal sealed class CreateTrainingRecommendationRequestCommandHandler
    : IRequestHandler<CreateTrainingRecommendationRequestCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateTrainingRecommendationRequestCommandHandler> _logger;
    private readonly TrainingDbContext _trainingDbContext;

    public CreateTrainingRecommendationRequestCommandHandler(
        TrainingDbContext trainingDbContext,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _trainingDbContext = trainingDbContext;
    }

    public async Task<Guid> Handle(CreateTrainingRecommendationRequestCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _trainingDbContext.Projects
                          .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
                      ?? throw new ProjectNotFoundException($"Could not find project with id : <{request.ProjectId}>");

        TrainingRecommendationRequest recommendationRequest = new()
        {
            Project = project,
            Title = request.Title,
            RequestStatus = RequestStatus.Accepted
        };

        await _trainingDbContext.AddAsync(recommendationRequest, cancellationToken);
        await _trainingDbContext.SaveChangesAsync(cancellationToken);

        var requestAcceptedEvent = _mapper.Map<TrainingRecommendationRequestAcceptedEvent>(recommendationRequest);
        await _publishEndpoint.Publish(requestAcceptedEvent, cancellationToken);

        return recommendationRequest.Id;
    }
}