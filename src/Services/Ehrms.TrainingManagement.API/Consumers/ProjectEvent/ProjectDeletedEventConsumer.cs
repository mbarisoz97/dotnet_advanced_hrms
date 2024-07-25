namespace Ehrms.TrainingManagement.API.Consumers.ProjectEvent;

public sealed class ProjectDeletedEventConsumer : IConsumer<ProjectDeletedEvent>
{
	private readonly TrainingDbContext _dbContext;
	private readonly ILogger<ProjectDeletedEventConsumer> _logger;

	public ProjectDeletedEventConsumer(TrainingDbContext dbContext, ILogger<ProjectDeletedEventConsumer> logger)
	{
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
	{
		//Ignore if event is not valid
		var projectDeletedEvent = context.Message;
		if (projectDeletedEvent == null)
		{
			_logger.LogError("Ignored null project deleted event");
			return;
		}

		//Ignore event if project does not exist
		_logger.LogInformation("Consuming project deleted event with id : {id}", context.Message.Id);
		var project = _dbContext.Projects.FirstOrDefault(x => x.Id == projectDeletedEvent.Id);
		if (project == null)
		{
			_logger.LogError("Could not find project with id : {id}", projectDeletedEvent.Id);
			return;
		}

		//Remove project from database
		_dbContext.Remove(project);
		await _dbContext.SaveChangesAsync();
		_logger.LogInformation("Consumed project deleted event with id : {id}", projectDeletedEvent.Id);
	}
}