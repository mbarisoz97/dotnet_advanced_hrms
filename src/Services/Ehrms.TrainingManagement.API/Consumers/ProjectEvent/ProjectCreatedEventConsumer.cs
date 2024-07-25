namespace Ehrms.TrainingManagement.API.Consumers.ProjectEvent;

public sealed class ProjectCreatedEventConsumer : IConsumer<ProjectCreatedEvent>
{
	private readonly IMapper _mapper;
	private readonly ILogger<ProjectCreatedEventConsumer> _logger;

	private readonly TrainingDbContext _dbContext;

	public ProjectCreatedEventConsumer(ILogger<ProjectCreatedEventConsumer> logger, IMapper mapper, TrainingDbContext dbContext)
	{
		_logger = logger;
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
	{
		_logger.LogDebug("Consuming project created event");

		var projectCreatedEvent = context.Message;

		var project = _mapper.Map<Project>(context.Message);

		project.RequiredSkills = await _dbContext.Skills
			.Where(x => projectCreatedEvent.RequiredSkills.Contains(x.Id))
			.ToListAsync();

		project.Employees = await _dbContext.Employees
			.Where(x => projectCreatedEvent.Employees.Contains(x.Id))
			.ToListAsync();

		await _dbContext.Projects.AddAsync(project);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("Consumed project created event with id : {id}", context.Message.Id);
	}
}
