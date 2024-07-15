using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class CreateEmployeeCommand : IRequest<Database.Models.Employee>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Database.Models.Employee>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateEmployeeCommandHandler(IPublishEndpoint publishEndpoint, IMapper mapper, EmployeeInfoDbContext dbContext)
    {
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Database.Models.Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Database.Models.Employee employee = _mapper.Map<Database.Models.Employee>(request);

        await _dbContext.Skills
            .Where(x => request.Skills.Contains(x.Id))
            .ForEachAsync(employee.Skills.Add);

        await _dbContext.Employees.AddAsync(employee, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var employeeCreatedEvent = _mapper.Map<EmployeeCreatedEvent>(employee);
        await _publishEndpoint.Publish(employeeCreatedEvent, cancellationToken);

        return employee;
    }
}