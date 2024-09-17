using AutoMapper;
using Ehrms.Contracts.Employee;
using Ehrms.Administration.API.Database.Context;

namespace Ehrms.Administration.API.Consumer;

public class EmployeeUpdatedEventConsumer : IConsumer<EmployeeUpdatedEvent>
{
    private readonly AdministrationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeUpdatedEventConsumer> _logger;

    public EmployeeUpdatedEventConsumer(
        AdministrationDbContext dbContext,
        IMapper mapper,
        ILogger<EmployeeUpdatedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
    {
        if (context == null)
        {
            _logger.LogError("Null event context ignored by consumer.");
            return;
        }

        var message = context.Message;
        if (message == null)
        {
            _logger.LogError("Null <{0}> event ignored by consumer.", nameof(EmployeeUpdatedEvent));
            return;
        }

        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == message.Id);
        if (employee == null)
        {
            _logger.LogError("Could not find employee with id : <{Id}>", message.Id);
            return;
        }

        _mapper.Map(message, employee);

        _dbContext.Update(employee);
        await _dbContext.SaveChangesAsync();
    }
}