namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

public sealed class UpdateProjectCommand : IRequest<Models.Project>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<Guid> Employees { get; set; } = [];
}

internal sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Models.Project>
{
	private readonly IMapper _mapper;
	private readonly ProjectDbContext _dbContext;

	public UpdateProjectCommandHandler(IMapper mapper, ProjectDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Models.Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
	{
		Models.Project project = await GetProject(request.Id);
		_mapper.Map(request, project);

		await SetEmploymentEndDateForRemovedEmployees(project, request.Employees);
		await CreateEmploymenRecordsForNewEmployees(project, request.Employees);

		_dbContext.Update(project);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return project;
	}

	private async Task<Models.Project> GetProject(Guid id)
	{
		return await _dbContext.Projects
			.Include(x => x.Employments)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new ProjectNotFoundException($"Could not find project with id '{id}'");
	}

	private async Task CreateEmploymenRecordsForNewEmployees(Models.Project project, ICollection<Guid> employeeIdCollection)
	{
		var currentProjectEmployments = _dbContext.Employments
			.Where(x => x.EndedAt == null && x.ProjectId == project.Id)
			.Select(x => x.EmployeeId);

		var newEmploymentRecords = employeeIdCollection
			.Where(x => !currentProjectEmployments.Contains(x));

		var employeesCreateToEmploymentRecord = _dbContext.Employees.Where(x => newEmploymentRecords.Contains(x.Id));
		foreach (var employee in employeesCreateToEmploymentRecord)
		{
			_dbContext.Employments.Add(new Models.Employment
			{
				Employee = employee,
				Project = project,
			});
		}
		await _dbContext.SaveChangesAsync();
	}

	private async Task SetEmploymentEndDateForRemovedEmployees(Models.Project project, ICollection<Guid> employeeIdCollection)
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
}