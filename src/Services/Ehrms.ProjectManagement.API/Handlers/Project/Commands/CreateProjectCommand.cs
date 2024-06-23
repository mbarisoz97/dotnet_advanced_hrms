namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

internal sealed class CreateProjectCommand : IRequest<Models.Project>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
internal sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Models.Project>
{
    private readonly IMapper _mapper;
    private readonly ProjectDbContext _dbContext;

    public CreateProjectCommandHandler(IMapper mapper, ProjectDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Models.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = _mapper.Map<Models.Project>(request);
     
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
        
        return project;
    }

}