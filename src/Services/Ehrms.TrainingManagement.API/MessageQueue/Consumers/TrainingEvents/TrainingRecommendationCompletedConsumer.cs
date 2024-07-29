namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TrainingEvents;

public sealed class TrainingRecommendationCompletedConsumer : IConsumer<TrainingRecommendationCompletedEvent>
{
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<TrainingRecommendationCompletedConsumer> _logger;

    public TrainingRecommendationCompletedConsumer(TrainingDbContext dbContext, ILogger<TrainingRecommendationCompletedConsumer> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TrainingRecommendationCompletedEvent> context)
    {
        var request = await _dbContext.RecommendationRequests
            .FirstOrDefaultAsync(x => x.Id == context.Message.RequestId);

        if (request == null)
        {
            _logger.LogError("Could not find recommendation request with id : <{id}>", context.Message.RequestId);
            return;
        }
        
        request.RequestStatus = RequestStatus.Completed;

        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
    }
}