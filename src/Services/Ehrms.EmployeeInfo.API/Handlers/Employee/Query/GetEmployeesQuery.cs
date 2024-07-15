using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Query;

public sealed class GetEmployeesQuery : IRequest<IQueryable<Database.Models.Employee>>
{
}

internal sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IQueryable<Database.Models.Employee>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public GetEmployeesQueryHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IQueryable<Database.Models.Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dbContext.Employees.AsQueryable());
    }
}