namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class DeleteEmployeeCommand : IRequest
{
    public Guid Id { get; set; }
}

internal sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public DeleteEmployeeCommandHandler(EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == request.Id)
             ?? throw new EmployeeNotFoundException($"Could not find employee with id '{request.Id}'");

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }
}