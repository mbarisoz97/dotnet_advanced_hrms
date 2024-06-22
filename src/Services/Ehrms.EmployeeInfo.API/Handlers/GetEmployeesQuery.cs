namespace Ehrms.EmployeeInfo.API.Handlers;

public sealed class GetEmployeesQuery : IRequest<IQueryable<Employee>>
{
}

internal sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IQueryable<Employee>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetEmployeesQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IQueryable<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dbContext.Employees.AsQueryable());
    }
}