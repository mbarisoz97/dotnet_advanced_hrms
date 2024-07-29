using Ehrms.Contracts.Skill;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.SkillEvent;

internal sealed class SkillCreatedEventConsumer : IConsumer<SkillCreatedEvent>
{
	private readonly IMapper _mapper;
	private readonly ILogger<SkillCreatedEventConsumer> _logger;
	
	private readonly TrainingDbContext _dbContext;

	public SkillCreatedEventConsumer(ILogger<SkillCreatedEventConsumer> logger, IMapper mapper, TrainingDbContext dbContext)
    {
		_logger = logger;
		_mapper = mapper;
		_dbContext = dbContext;
	}
    public async Task Consume(ConsumeContext<SkillCreatedEvent> context)
	{
		_logger.LogDebug("Consuming skill created event");

		var skill = _mapper.Map<Skill>(context.Message);
		
		await _dbContext.Skills.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Consume skill created event with id : {id}", context.Message.Id);
	}
}
