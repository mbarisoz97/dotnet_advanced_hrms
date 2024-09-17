using Ehrms.Contracts.Employee;
using Ehrms.Administration.API.Database.Context;

namespace Ehrms.Administration.API.Consumer;

public class EmployeeDeletedEventConsumer : IConsumer<EmployeeDeletedEvent>
{
    private readonly AdministrationDbContext _dbContext;
    private readonly ILogger<EmployeeDeletedEventConsumer> _logger;

    public EmployeeDeletedEventConsumer(AdministrationDbContext dbContext, ILogger<EmployeeDeletedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeDeletedEvent> context)
    {
        if (context == null)
        {
            _logger.LogError("Event consumer ignored employee deleted event. " +
                             "Consume context was null");
            return;
        }

        if (context.Message == null)
        {
            _logger.LogError("Event consumer ignored employee deleted event. " +
                             "Consume context message was null.");
            return;
        }


        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == context.Message.Id);
        if (employee == null)
        {
            _logger.LogError("Event consumer ignored employee deleted event. " +
                             "Could not find employee with id {id}", context.Message.Id);
            return;
        }

        _dbContext.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }
}