namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

public sealed class CreateProjectCommand : IRequest<Database.Models.Project>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<Guid> Employees { get; set; } = [];
	public ICollection<Guid> RequiredSkills { get; set; } = [];
}

internal sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Database.Models.Project>
{
	private readonly IMapper _mapper;
	private readonly ProjectDbContext _dbContext;

	public CreateProjectCommandHandler(IMapper mapper, ProjectDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Database.Models.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
	{
		var project = _mapper.Map<Database.Models.Project>(request);
		project.Employments = await GetEmployments(project, request.Employees);
		project.RequiredProjectSkills = await GetSkillsAsync(project, request.RequiredSkills);

		await _dbContext.Projects.AddAsync(project, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
		return project;
	}

	private async Task<ICollection<Database.Models.Employment>> GetEmployments(Database.Models.Project project, ICollection<Guid> employees)
	{
		if (employees.Count == 0)
		{
			return [];
		}

		var projectEmployees = _dbContext.Employees
			.Where(x => employees.Contains(x.Id));

		foreach (var employee in projectEmployees)
		{
			var newEmployment = CreateNewEmploymentRecord(project, employee);
			project.Employments.Add(newEmployment);
		}

		return await Task.FromResult(project.Employments);
	}

	private async Task<ICollection<Skill>> GetSkillsAsync(Database.Models.Project project, ICollection<Guid> skills)
	{
		if (skills.Count == 0)
		{
			return [];
		}

		return _dbContext.Skills
			.Where(x => skills.Contains(x.Id))
			.ToList();
	}

	private static Database.Models.Employment CreateNewEmploymentRecord(Database.Models.Project project, Employee employee) => new()
	{
		StartedAt = DateOnly.FromDateTime(DateTime.UtcNow),
		Employee = employee,
		Project = project
	};
}