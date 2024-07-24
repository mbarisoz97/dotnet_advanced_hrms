using Ehrms.Contracts.Project;

namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

public sealed class UpdateProjectCommand : IRequest<Database.Models.Project>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<Guid> Employees { get; set; } = [];
	public ICollection<Guid> RequiredSkills { get; set; } = [];
}

internal sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Database.Models.Project>
{
	private readonly ProjectDbContext _dbContext;

	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;

	public UpdateProjectCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, ProjectDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
		_publishEndpoint = publishEndpoint;
	}

	public async Task<Database.Models.Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
	{
		var project = await UpdateProject(request, cancellationToken);
		await PublishProjectEvent(project, cancellationToken);

		return project;
	}

	private async Task PublishProjectEvent(Database.Models.Project project, CancellationToken cancellationToken)
	{
		var projectUpdateEvent = _mapper.Map<ProjectUpdatedEvent>(project);
		await _publishEndpoint.Publish(projectUpdateEvent, cancellationToken);
	}

	private async Task<Database.Models.Project> UpdateProject(UpdateProjectCommand request, CancellationToken cancellationToken)
	{
		Database.Models.Project project = await GetProject(request.Id);
		_mapper.Map(request, project);

		await SetEmploymentEndDateForRemovedEmployees(project, request.Employees);
		await CreateEmploymenRecordsForNewEmployees(project, request.Employees);
		await SetRequiredProjectSkils(project, request.RequiredSkills);

		_dbContext.Update(project);
		await _dbContext.SaveChangesAsync(cancellationToken);
		return project;
	}

	private async Task<Database.Models.Project> GetProject(Guid id)
	{
		return await _dbContext.Projects
			.Include(x => x.Employments)
			.Include(x => x.RequiredProjectSkills)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new ProjectNotFoundException($"Could not find project with id '{id}'");
	}
	private async Task CreateEmploymenRecordsForNewEmployees(Database.Models.Project project, ICollection<Guid> employeeIdCollection)
	{
		var currentProjectEmployments = _dbContext.Employments
			.Where(x => x.EndedAt == null && x.ProjectId == project.Id)
			.Select(x => x.EmployeeId);

		var newEmploymentRecords = employeeIdCollection
			.Where(x => !currentProjectEmployments.Contains(x));

		var employeesCreateToEmploymentRecord = _dbContext.Employees.Where(x => newEmploymentRecords.Contains(x.Id));
		foreach (var employee in employeesCreateToEmploymentRecord)
		{
			_dbContext.Employments.Add(new Database.Models.Employment
			{
				Employee = employee,
				Project = project,
			});
		}
		await _dbContext.SaveChangesAsync();
	}
	private async Task SetEmploymentEndDateForRemovedEmployees(Database.Models.Project project, ICollection<Guid> employeeIdCollection)
	{
		var employmentRecordsToEnd = _dbContext.Employments
			.Include(x => x.Project)
			.Include(x => x.Employee)
			.Where(x => x.Project!.Id == project.Id &&
						x.EndedAt == null &&
						!employeeIdCollection.Contains(x.Employee!.Id))
			.AsQueryable();

		foreach (var employmentRecord in employmentRecordsToEnd)
		{
			employmentRecord.EndedAt = DateOnly.FromDateTime(DateTime.Now);
		}

		_dbContext.UpdateRange(employmentRecordsToEnd);
		await _dbContext.SaveChangesAsync();
	}
	private async Task SetRequiredProjectSkils(Database.Models.Project project, ICollection<Guid> requiredSkillIdentifiers)
	{
		var skillsToRemove =  _dbContext.Skills
			.Where(x => !requiredSkillIdentifiers.Contains(x.Id));

		await skillsToRemove.ForEachAsync(x => project.RequiredProjectSkills.Remove(x));

		var skillsToAdd =  _dbContext.Skills
			.Where(x => requiredSkillIdentifiers.Contains(x.Id) &&
					   !project.RequiredProjectSkills.Contains(x));

		await skillsToAdd.ForEachAsync(project.RequiredProjectSkills.Add);
	}
}