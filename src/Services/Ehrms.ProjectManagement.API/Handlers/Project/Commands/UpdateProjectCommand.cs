namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

internal sealed class UpdateProjectCommand : IRequest<Models.Project>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Guid> EmployeeIdCollection { get; set; } = [];
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

        await SetEmploymentEndDateForRemovedEmployees(project, request.EmployeeIdCollection);
        await CreateEmploymenRecordsForNewEmployees(project, request.EmployeeIdCollection);

        _dbContext.Update(project);
        await _dbContext.SaveChangesAsync();

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
        var currentProjectEmployments = project.Employments
            .Where(x => x.EndedAt == null)
            .Select(x => x.Employee?.Id);

        var newEmploymentRecords = employeeIdCollection
            .Where(x => !currentProjectEmployments.Contains(x));

        var employeesCreateToEmploymentRecord = _dbContext.Employees.Where(x => newEmploymentRecords.Contains(x.Id));
        foreach (var employee in employeesCreateToEmploymentRecord)
        {
            _dbContext.Employments.Add(new Employment
            {
                Employee = employee,
                Project = project,
            });
        }
        await _dbContext.SaveChangesAsync();
    }

    private async Task SetEmploymentEndDateForRemovedEmployees(Models.Project project, ICollection<Guid> employeeIdCollection)
    {
        var employmentRecordsToEnd = project.Employments
            .Where(x => x.EndedAt == null) // active employments
            .Where(x=> !employeeIdCollection.Contains(x.Employee!.Id)) //employments to end
            .AsQueryable();

        foreach (var employmentRecord in employmentRecordsToEnd)
        {
            employmentRecord.EndedAt = DateOnly.FromDateTime(DateTime.Now);
        }

        _dbContext.UpdateRange(employmentRecordsToEnd);
        await _dbContext.SaveChangesAsync();
    }
}