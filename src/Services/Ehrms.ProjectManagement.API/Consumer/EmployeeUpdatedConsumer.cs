namespace Ehrms.ProjectManagement.API.Consumer;

internal sealed class EmployeeUpdatedConsumer : IConsumer<EmployeeUpdatedEvent>
{
	private readonly ProjectDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<EmployeeUpdatedConsumer> _logger;

	public EmployeeUpdatedConsumer(ILogger<EmployeeUpdatedConsumer> logger, IMapper mapper, ProjectDbContext dbContext)
	{
		_logger = logger;
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
	{
		var employee = await _dbContext.Employees
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == context.Message.Id);

		if (employee != null)
		{
			_mapper.Map(context.Message, employee);
			_dbContext.Update(employee);
			await _dbContext.SaveChangesAsync(context.CancellationToken);

			_logger.LogInformation("Updated employee with id '{id}'", context.Message.Id);
		}
		else
		{
			_logger.LogError("Could not find employee with id {Id}", context.Message.Id);
		}
	}
}