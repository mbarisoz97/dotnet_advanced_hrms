﻿namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Query;

public sealed class GetEmployeeByIdQuery : IRequest<Models.Employee>
{
    public Guid Id { get; set; }
}

internal sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Models.Employee>
{
    private readonly EmployeeInfoDbContext _context;

    public GetEmployeeByIdQueryHandler(EmployeeInfoDbContext context)
    {
        _context = context;
    }

    public async Task<Models.Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        Models.Employee employee = await _context.Employees
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
           ?? throw new ArgumentException($"Could not find employee with id {request.Id}");

        return employee;
    }
}