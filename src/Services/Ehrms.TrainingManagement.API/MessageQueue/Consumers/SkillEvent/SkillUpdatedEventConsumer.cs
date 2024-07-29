using Ehrms.Contracts.Skill;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.SkillEvent;

internal sealed class SkillUpdatedEventConsumer : IConsumer<SkillUpdatedEvent>
{
	private readonly TrainingDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<SkillUpdatedEventConsumer> _logger;

	public SkillUpdatedEventConsumer(IMapper mapper, ILogger<SkillUpdatedEventConsumer> logger, TrainingDbContext dbContext)
	{
		_mapper = mapper;
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<SkillUpdatedEvent> context)
	{
		var skillUpdatedEvent = context.Message;
		if (skillUpdatedEvent == null)
		{
			_logger.LogError("Null skill update event ignored");
			return;
		}

		var skill = await _dbContext.Skills
			.FirstOrDefaultAsync(x => x.Id == skillUpdatedEvent.Id);

		if (skill == null)
		{
			_logger.LogError("Could not find skill with id : {id}", skillUpdatedEvent.Id);
			return;
		}

		_mapper.Map(skillUpdatedEvent, skill);

		_dbContext.Update(skill);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Consumed skill updated event with id : {id}", skillUpdatedEvent.Id);
	}
}