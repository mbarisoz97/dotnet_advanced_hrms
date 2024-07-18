using Ehrms.ProjectManagement.API.Database.Context;
using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.Consumer.EmployeeEvents;

internal sealed class EmployeeCreatedConsumer : IConsumer<EmployeeCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeCreatedEvent> _logger;
    private readonly ProjectDbContext _projectDbContext;

    public EmployeeCreatedConsumer(ILogger<EmployeeCreatedEvent> logger, IMapper mapper, ProjectDbContext projectDbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _projectDbContext = projectDbContext;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        _logger.LogDebug("Consuming employee created event.");

        Employee employee = _mapper.Map<Employee>(context.Message);

        await _projectDbContext.AddAsync(employee);
        await _projectDbContext.SaveChangesAsync();

        _logger.LogInformation("Employee created event consumed.");
    }
}
