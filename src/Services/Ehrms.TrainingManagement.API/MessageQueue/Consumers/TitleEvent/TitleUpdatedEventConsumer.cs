using Ehrms.Contracts.Title;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;

public class TitleUpdatedEventConsumer : IConsumer<TitleUpdatedEvent>
{
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<TitleUpdatedEventConsumer> _logger;

    public TitleUpdatedEventConsumer(IMapper mapper, TrainingDbContext dbContext, ILogger<TitleUpdatedEventConsumer> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TitleUpdatedEvent> context)
    {
        var existingTitle = _dbContext.Titles.FirstOrDefault(x => x.Id == context.Message.Id);
        if (existingTitle == null)
        {
            _logger.LogError("Ignored title created event. Could not find title with id <{id}>", context.Message.Id);
            return;
        }

        _mapper.Map(context.Message,existingTitle);

        _dbContext.Update(existingTitle);
        await _dbContext.SaveChangesAsync();
    }
}