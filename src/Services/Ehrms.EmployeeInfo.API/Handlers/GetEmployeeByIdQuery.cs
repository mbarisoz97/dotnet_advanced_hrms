
using Microsoft.EntityFrameworkCore;

namespace Ehrms.EmployeeInfo.API.Handlers;

public sealed class GetEmployeeByIdQuery : IRequest<Employee>
{
    public Guid Id { get; set; }
}

internal sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
{
    private readonly EmployeeInfoDbContext _context;

    public GetEmployeeByIdQueryHandler(EmployeeInfoDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
           ?? throw new ArgumentException($"Could not find employee with id {request.Id}");

        return employee;
    }
}