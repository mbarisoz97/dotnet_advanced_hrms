namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TrainingEvents;

public sealed class
    TrainingRecommendationRequestAcceptedEventConsumer : IConsumer<TrainingRecommendationRequestAcceptedEvent>
{
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<TrainingRecommendationRequestAcceptedEventConsumer> _logger;

    public TrainingRecommendationRequestAcceptedEventConsumer(TrainingDbContext dbContext,
        ILogger<TrainingRecommendationRequestAcceptedEventConsumer> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        var request = await _dbContext.RecommendationRequests.FirstOrDefaultAsync(x => x.Id == context.Message.RequestId);
        if (request == null)
        {
            _logger.LogError("Ignored null event : <{event}>", nameof(TrainingRecommendationRequestAcceptedEvent));
            return;
        }

        var project = await _dbContext.Projects
            .Include(x => x.Employees)
            .Include(x => x.RequiredSkills)
            .FirstOrDefaultAsync(x => x.Id == request.ProjectId);

        if (project == null)
        {
            _logger.LogError("Could not find project with id : <{id}>", request.ProjectId);
            await PublishRecommendationCancelledEvent(context);
            return;
        }

        await SetRequestStatusAsPending(request);
        await CreateTrainingRecommendations(request, project);
        await PublishRecommendationCompletedEvent(context);
    }

    private async Task SetRequestStatusAsPending(TrainingRecommendationRequest request)
    {
        request.RequestStatus = RequestStatus.Pending;
        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    private async Task CreateTrainingRecommendations(TrainingRecommendationRequest request, Project project)
    {
        foreach (var requiredSkill in project.RequiredSkills)
        {
            TrainingRecommendationResult recommendationResult = new()
            {
                Skill = requiredSkill,
                RecommendationRequest = request
            };
            
            foreach (var employee in project.Employees)
            {
                if (!employee.Skills.Contains(requiredSkill))
                {
                    recommendationResult.Employees.Add(employee);
                }
            }
            
            if (recommendationResult.Employees.Count > 0)
            {
                await _dbContext.AddAsync(recommendationResult);
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    private static async Task PublishRecommendationCompletedEvent(
        ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        TrainingRecommendationCompletedEvent recommendationCompletedEvent = new()
        {
            RequestId = context.Message.RequestId
        };
        await context.Publish(recommendationCompletedEvent);
    }

    private static async Task PublishRecommendationCancelledEvent(
        ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        TrainingRecommendationCancelledEvent recommendationCancelledEvent =
            new() { RequestId = context.Message.RequestId };
        await context.Publish(recommendationCancelledEvent);
    }
}