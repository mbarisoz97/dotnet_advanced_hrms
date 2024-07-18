using Ehrms.Contracts.Skill;

namespace Ehrms.ProjectManagement.API.Consumer.SkillEvents;

internal sealed class SkillDeletedEventConsumer : IConsumer<SkillDeletedEvent>
{
	private readonly ILogger<SkillDeletedEventConsumer> _logger;
	private readonly ProjectDbContext _dbContext;

	public SkillDeletedEventConsumer(ILogger<SkillDeletedEventConsumer> logger, ProjectDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<SkillDeletedEvent> context)
	{
		_logger.LogDebug("Consuming skill deleted event");
		var skillDeletedEvent = context.Message;
		var skill = _dbContext.Skills.FirstOrDefault(x => x.Id == skillDeletedEvent.Id);

		if (skill == null)
		{
			_logger.LogError("Could not find skill with id : {id}", skillDeletedEvent.Id);
			return;
		}

		_dbContext.Remove(skill);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Consumed skill deleted event with id : {id}", skillDeletedEvent.Id);
	}
}