using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

public class EmployeeUpdatedEventConsumer : IConsumer<EmployeeUpdatedEvent>
{
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<EmployeeUpdatedEventConsumer> _logger;

    public EmployeeUpdatedEventConsumer(IMapper mapper, TrainingDbContext dbContext,
        ILogger<EmployeeUpdatedEventConsumer> logger)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
    {
        var updateEvent = context.Message;
        var employee = await _dbContext.Employees
            .Include(x => x.Skills)
            .FirstOrDefaultAsync(employee => employee.Id == updateEvent.Id);

        if (employee == null)
        {
            _logger.LogError("Could not find employee with id {id}", updateEvent.Id);
            return;
        }

        employee = _mapper.Map(context.Message, employee);
        await UpdateEmployeeSkills(updateEvent, employee);
        employee.Title = _dbContext.Titles
            .FirstOrDefault(x=>x.Id == context.Message.TitleId);
        
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Updated employee with id {id}", updateEvent.Id);
    }

    private Task UpdateEmployeeSkills(EmployeeUpdatedEvent employeeUpdatedEvent, Employee employee)
    {
        var updatedEmployeeSkills = _dbContext.Skills.Where(x => employeeUpdatedEvent.Skills.Contains(x.Id));

        var skillsToRemove = employee.Skills
            .Where(x => !updatedEmployeeSkills.Contains(x))
            .ToList();
        foreach (var skill in skillsToRemove)
        {
            employee.Skills.Remove(skill);
        }

        var skillsToAdd = updatedEmployeeSkills
            .Where(x => !employee.Skills.Contains(x))
            .ToList();
        foreach (var skill in skillsToAdd)
        {
            if (!employee.Skills.Contains(skill))
            {
                employee.Skills.Add(skill);
            }
        }

        return Task.CompletedTask;
    }
}