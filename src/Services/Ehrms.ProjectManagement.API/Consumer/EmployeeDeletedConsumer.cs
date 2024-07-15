using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.Consumer;

internal sealed class EmployeeDeletedConsumer : IConsumer<EmployeeDeletedEvent>
{
    private readonly ILogger<EmployeeDeletedConsumer> _logger;
    private readonly ProjectDbContext _dbContext;

    public EmployeeDeletedConsumer(ILogger<EmployeeDeletedConsumer> logger, ProjectDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EmployeeDeletedEvent> context)
    {
        var employee = _dbContext.Employees
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == context.Message.Id);

        if (employee != null)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}