using System.Runtime.Intrinsics.X86;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.ProjectEvent;

public sealed class ProjectUpdatedEventConsumer : IConsumer<ProjectUpdatedEvent>
{
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectUpdatedEventConsumer> _logger;
    private readonly TrainingDbContext _dbContext;

    public ProjectUpdatedEventConsumer(ILogger<ProjectUpdatedEventConsumer> logger, IMapper mapper, TrainingDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProjectUpdatedEvent> context)
    {
        _logger.LogDebug("Consuming project updated event");
        var projectUpdatedEvent = context.Message;

        if (projectUpdatedEvent == null)
        {
            _logger.LogError("Ignored null project updated event");
            return;
        }

        var project = await _dbContext.Projects
            .Include(x => x.Employees)
            .Include(x => x.RequiredSkills)
            .FirstOrDefaultAsync(x => x.Id == projectUpdatedEvent.Id);

        if (project == null)
        {
            _logger.LogError("Could not find project with id : {Id}", projectUpdatedEvent.Id);
            return;
        }

        project = _mapper.Map(projectUpdatedEvent, project);
        await UpdateProjectEmployees(projectUpdatedEvent, project);
        await UpdateRequiredProjectSkills(projectUpdatedEvent, project);

        _dbContext.Update(project);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Consumed project updated event with id : {id}", context.Message.Id);
    }

    private Task UpdateRequiredProjectSkills(ProjectUpdatedEvent projectUpdatedEvent, Project project)
    {
        var updatedProjectSkills = _dbContext.Skills.Where(x => projectUpdatedEvent.RequiredSkills.Contains(x.Id));
        
        var skillsToRemove = project.RequiredSkills.Where(x => !updatedProjectSkills.Contains(x)).ToList();
        foreach (var skill in skillsToRemove)
        {
            project.RequiredSkills.Remove(skill);
        }

        foreach (var skill in updatedProjectSkills)
        {
            if (!project.RequiredSkills.Contains(skill))
            {
                project.RequiredSkills.Add(skill);
            }
        }

        return Task.CompletedTask;
    }

    private Task UpdateProjectEmployees(ProjectUpdatedEvent projectUpdatedEvent, Project project)
    {
        var updatedProjectEmployees = _dbContext.Employees.Where(x => projectUpdatedEvent.Employees.Contains(x.Id));

        var employeesToRemove = project.Employees.Where(x => !updatedProjectEmployees.Contains(x));
        foreach (var employee in employeesToRemove)
        {
            project.Employees.Remove(employee);
        }

        foreach (var employee in updatedProjectEmployees)
        {
            if (!project.Employees.Contains(employee))
            {
                project.Employees.Add(employee);
            }
        }

        return Task.CompletedTask;
    }
}