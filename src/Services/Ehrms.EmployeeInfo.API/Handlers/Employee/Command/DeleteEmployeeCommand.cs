using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class DeleteEmployeeCommand : IRequest
{
    public Guid Id { get; set; }
}

internal sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly EmployeeInfoDbContext _dbContext;

    public DeleteEmployeeCommandHandler(IPublishEndpoint publishEndpoint, EmployeeInfoDbContext dbContext)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = _dbContext.Employees
            .AsNoTracking()
            .FirstOrDefault(x=>x.Id == request.Id)
             ?? throw new EmployeeNotFoundException($"Could not find employee with id '{request.Id}'");

        _dbContext.Remove(employee);
        await _dbContext.SaveChangesAsync();

        await _publishEndpoint.Publish(new EmployeeDeletedEvent { Id = employee.Id }, cancellationToken);
    }
}