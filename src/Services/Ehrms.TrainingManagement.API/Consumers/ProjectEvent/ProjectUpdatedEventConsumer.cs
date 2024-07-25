namespace Ehrms.TrainingManagement.API.Consumers.ProjectEvent;

public sealed class ProjectUpdatedEventConsumer : IConsumer<ProjectUpdatedEvent>
{
	private readonly IMapper _mapper;
	private readonly ILogger<ProjectUpdatedEventConsumer> _logger;
	private readonly TrainingDbContext _dbContext;

	public ProjectUpdatedEventConsumer(ILogger<ProjectUpdatedEventConsumer> logger, IMapper mapper, TrainingDbContext dbContext)
	{
		_logger = logger;
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<ProjectUpdatedEvent> context)
	{
		_logger.LogDebug("Consuming project updated event");
		var projectUpdatedEvent = context.Message;

		if (projectUpdatedEvent == null)
		{
			_logger.LogError("Ignored null project updated event");
			return;
		}

		var project = _dbContext.Projects.FirstOrDefault(x => x.Id == projectUpdatedEvent.Id);

		if (project == null)
		{
			_logger.LogError("Could not find project with id : {Id}", projectUpdatedEvent.Id);
			return;
		}

		_mapper.Map(projectUpdatedEvent, project);

		project.Employees = await _dbContext.Employees
			.Where(x => projectUpdatedEvent.Employees.Contains(x.Id))
			.ToListAsync();

		project.RequiredSkills = await _dbContext.Skills
			.Where(x => projectUpdatedEvent.RequiredSkills.Contains(x.Id))
			.ToListAsync();

		_dbContext.Update(project);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Consumed project updated event with id : {id}", context.Message.Id);
	}
}