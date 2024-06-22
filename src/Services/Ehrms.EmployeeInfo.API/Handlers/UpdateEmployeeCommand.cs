namespace Ehrms.EmployeeInfo.API.Handlers;

public sealed class UpdateEmployeeCommand : IRequest<Employee>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;

    public UpdateEmployeeCommandHandler(EmployeeInfoDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new ArgumentException($"Could not find employee with id {request.Id}");

        _mapper.Map(request, employee);
        employee = SetEmployeeSkills(employee, request.Skills);
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return employee;
    }

    private static Employee SetEmployeeSkills(Employee employee, ICollection<Guid> employeeSkills)
    {
        var skillToRemove = employee.Skills
            .Where(x => !employeeSkills.Contains(x.Id));

        foreach (var skill in skillToRemove)
        {
            employee.Skills.Remove(skill);
        }

        return employee;
    }
}