using Ehrms.Contracts.Title;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TitleEvent;

public class TitleCreatedEventConsumer : IConsumer<TitleCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<TitleCreatedEventConsumer> _logger;

    public TitleCreatedEventConsumer(IMapper mapper, TrainingDbContext dbContext, ILogger<TitleCreatedEventConsumer> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TitleCreatedEvent> context)
    {
        var existingTitle = _dbContext.Titles.FirstOrDefault(x=>x.Name == context.Message.TitleName);
        if (existingTitle != null)
        {
            _logger.LogError("Ignored title created event. A title with same name : {name} already exists", context.Message.TitleName);
            return;
        }

        var newTitle = _mapper.Map<Title>(context.Message);
        await _dbContext.AddAsync(newTitle);
        await _dbContext.SaveChangesAsync();
    }
}
