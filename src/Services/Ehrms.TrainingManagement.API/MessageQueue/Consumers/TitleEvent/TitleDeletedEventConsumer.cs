using Ehrms.Contracts.Title;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;

public class TitleDeletedEventConsumer : IConsumer<TitleDeletedEvent>
{
    private readonly ILogger<TitleDeletedEventConsumer> _logger;
    private readonly TrainingDbContext _dbContext;

    public TitleDeletedEventConsumer(TrainingDbContext dbContext, ILogger<TitleDeletedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TitleDeletedEvent> context)
    {
        var existingTitle = _dbContext.Titles.FirstOrDefault(x=>x.Id == context.Message.Id);
        if (existingTitle == null)
        {
            _logger.LogError("Ignored title deleted event. Could not find title with id : <{id}>", context.Message.Id);
            return;
        }

        _dbContext.Remove(existingTitle);
        await _dbContext.SaveChangesAsync();
    }
}