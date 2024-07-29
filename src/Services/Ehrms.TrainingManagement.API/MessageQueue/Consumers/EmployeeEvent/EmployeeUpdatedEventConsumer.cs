using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

public class EmployeeUpdatedEventConsumer : IConsumer<EmployeeUpdatedEvent>
{
	private readonly IMapper _mapper;
	private readonly TrainingDbContext _dbContext;
	private readonly ILogger<EmployeeUpdatedEventConsumer> _logger;

	public EmployeeUpdatedEventConsumer(IMapper mapper, TrainingDbContext dbContext,
		ILogger<EmployeeUpdatedEventConsumer> logger)
	{
		_mapper = mapper;
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
	{
		var updateEvent = context.Message;
		var employee = await _dbContext.Employees
			.FirstOrDefaultAsync(employee => employee.Id == updateEvent.Id);

		if (employee != null)
		{
			_mapper.Map(context.Message, employee);
			_dbContext.Update(employee);
			await _dbContext.SaveChangesAsync();

			_logger.LogInformation("Updated employee with id {id}", updateEvent.Id);
		}
		else
		{
			_logger.LogError("Could not find employee with id {id}", updateEvent.Id);
		}
	}
}