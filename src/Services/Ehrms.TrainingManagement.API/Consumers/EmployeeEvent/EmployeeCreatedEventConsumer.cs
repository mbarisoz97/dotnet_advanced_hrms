using MassTransit;
using Ehrms.Contracts.Employee;
using Ehrms.TrainingManagement.API.Database.Models;
using Ehrms.TrainingManagement.API.Database.Context;

namespace Ehrms.TrainingManagement.API.Consumers.EmployeeEvent;

public class EmployeeCreatedEventConsumer : IConsumer<EmployeeCreatedEvent>
{
	private readonly IMapper _mapper;
	private readonly TrainingDbContext _dbContext;
	
	public EmployeeCreatedEventConsumer(IMapper mapper, TrainingDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
	{
		var employee = _mapper.Map<Employee>(context.Message);
		await _dbContext.AddAsync(employee);
		await _dbContext.SaveChangesAsync();
	}
}