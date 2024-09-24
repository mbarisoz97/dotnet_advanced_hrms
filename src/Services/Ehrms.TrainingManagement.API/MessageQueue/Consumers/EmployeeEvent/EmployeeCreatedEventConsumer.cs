using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

public class EmployeeCreatedEventConsumer : IConsumer<EmployeeCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _dbContext;

    public EmployeeCreatedEventConsumer(IMapper mapper, TrainingDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var employee = _mapper.Map<Employee>(context.Message);

        await _dbContext.Skills
            .Where(x => context.Message.Skills.Contains(x.Id))
            .ForEachAsync(employee.Skills.Add);

        employee.Title = _dbContext.Titles
            .FirstOrDefault(x => x.Id == context.Message.TitleId);

        await _dbContext.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
    }
}