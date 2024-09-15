using AutoMapper;
using Ehrms.Contracts.Employee;
using Ehrms.Administration.API.Database.Models;
using Ehrms.Administration.API.Database.Context;

namespace Ehrms.Administration.API.Consumer;

public class EmployeeCreatedEventConsumer : IConsumer<EmployeeCreatedEvent>
{
    private readonly AdministrationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeCreatedEventConsumer> _logger;

    public EmployeeCreatedEventConsumer(AdministrationDbContext dbContext, IMapper mapper, ILogger<EmployeeCreatedEventConsumer> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var message = context.Message;
        if (message == null)
        {
            _logger.LogError("Null <{0}> event ignored by consumer.", nameof(EmployeeCreatedEvent));
            return;
        }

        var employee = _mapper.Map<Employee>(message);
        
        await _dbContext.AddAsync(employee);    
        await _dbContext.SaveChangesAsync();    
    }
}