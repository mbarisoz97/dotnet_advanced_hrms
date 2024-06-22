namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class CreateEmployeeCommand : IRequest<Models.Employee>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Models.Employee>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateEmployeeCommandHandler(IMapper mapper, EmployeeInfoDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Models.Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Models.Employee employee = _mapper.Map<Models.Employee>(request);

        await _dbContext.Employees.AddAsync(employee, cancellationToken);


        await _dbContext.SaveChangesAsync(cancellationToken);

        return employee;
    }
}
