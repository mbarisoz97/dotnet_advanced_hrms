using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

public class EmployeeDeletedEventConsumer : IConsumer<EmployeeDeletedEvent>
{
	private readonly ILogger<EmployeeDeletedEventConsumer> _logger;
	private readonly TrainingDbContext _dbContext;

	public EmployeeDeletedEventConsumer(ILogger<EmployeeDeletedEventConsumer> logger, TrainingDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<EmployeeDeletedEvent> context)
	{
		var employee = await _dbContext.Employees
			.FirstOrDefaultAsync(x => x.Id == context.Message.Id);

		if (employee != null)
		{
			_dbContext.Remove(employee);
			await _dbContext.SaveChangesAsync();
			
			_logger.LogInformation("Removed employee with id {id}", context.Message.Id);
		}
		else
		{
			_logger.LogError("Could not find employee with id {employeeId}. Ignored event", context.Message.Id);
		}
	}
}