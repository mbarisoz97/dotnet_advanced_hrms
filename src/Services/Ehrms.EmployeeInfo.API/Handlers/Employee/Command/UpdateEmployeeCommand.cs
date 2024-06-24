namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class UpdateEmployeeCommand : IRequest<Models.Employee>
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Qualification { get; set; } = string.Empty;
	public DateOnly DateOfBirth { get; set; }
	public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Models.Employee>
{
	private readonly IMapper _mapper;
	private readonly EmployeeInfoDbContext _dbContext;
	private readonly IPublishEndpoint _publishEndpoint;

	public UpdateEmployeeCommandHandler(EmployeeInfoDbContext dbContext, IPublishEndpoint publishEndpoint, IMapper mapper)
	{
		_mapper = mapper;
		_dbContext = dbContext;
		_publishEndpoint = publishEndpoint;
	}

	public async Task<Models.Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
	{
		Models.Employee employee = await _dbContext.Employees
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EmployeeNotFoundException($"Could not find employee with id '{request.Id}'");

		_mapper.Map(request, employee);
		employee = SetEmployeeSkills(employee, request.Skills);

		_dbContext.Update(employee);
		await _dbContext.SaveChangesAsync(cancellationToken);

		var employeeUpdatedEvent = _mapper.Map<EmployeeUpdatedEvent>(employee);
		await _publishEndpoint.Publish(employeeUpdatedEvent, cancellationToken);

		return employee;
	}

	private Models.Employee SetEmployeeSkills(Models.Employee employee, ICollection<Guid> employeeSkills)
	{
		var skillToRemove = employee.Skills
			.Where(x => !employeeSkills.Contains(x.Id));

		foreach (var skill in skillToRemove)
		{
			employee.Skills.Remove(skill);
		}

		var skillsToAdd = _dbContext.Skills.AsNoTracking()
			.Where(x => employeeSkills
				.Contains(x.Id))
			.Where(s => !employee.Skills.Select(e => e.Id)
				.Contains(s.Id));

		foreach (var skill in skillsToAdd)
		{
			employee.Skills.Add(skill);
		}

		return employee;
	}
}