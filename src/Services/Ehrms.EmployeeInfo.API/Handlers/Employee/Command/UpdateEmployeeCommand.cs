using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class UpdateEmployeeCommand : IRequest<Result<Database.Models.Employee>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public Guid TitleId { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<Database.Models.Employee>>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateEmployeeCommandHandler(EmployeeInfoDbContext dbContext, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<Database.Models.Employee>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .Include(e => e.Skills)
            .Include(e => e.Title)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (employee == null)
        {
            return new Result<Database.Models.Employee>(
                new  EmployeeNotFoundException($"Could not find employee with id '{request.Id}'"));
        }
        
        var title = await _dbContext.Titles.FirstOrDefaultAsync(t=>t.Id == request.TitleId, cancellationToken);
        if (title == null)
        {
            return new Result<Database.Models.Employee>(
                new TitleNotFoundException($"Could not find title with id : <{request.Id}>"));
        }

        _mapper.Map(request, employee);
        employee.Title = title;
        employee = await SetSkills(employee, request.Skills);

        _dbContext.Update(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await PublishEmployeeUpdateEvent(employee, cancellationToken);

        return employee;
    }

    private async Task PublishEmployeeUpdateEvent(Database.Models.Employee updateEmployee, CancellationToken cancellationToken)
    {
        var employeeUpdatedEvent = _mapper.Map<EmployeeUpdatedEvent>(updateEmployee);
        await _publishEndpoint.Publish(employeeUpdatedEvent, cancellationToken);
    }

    private async Task<Database.Models.Employee> SetSkills(Database.Models.Employee employee, ICollection<Guid> skills)
    {
        RemoveSkills(employee, skills);
        await AddSkills(employee, skills);

        return employee;
    }

    private void RemoveSkills(Database.Models.Employee employee, ICollection<Guid> currentSkillIdentifiers)
    {
        var skillsToRemove = employee.Skills
            .Where(x => !currentSkillIdentifiers.Contains(x.Id))
            .ToList();

        foreach (var skill in skillsToRemove)
        {
            employee.Skills.Remove(skill);
        }
    }
    private Task AddSkills(Database.Models.Employee employee, ICollection<Guid> currentSkillIdentifiers)
    {
        var skillsToAdd = _dbContext.Skills
            .Where(x => currentSkillIdentifiers.Contains(x.Id));

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