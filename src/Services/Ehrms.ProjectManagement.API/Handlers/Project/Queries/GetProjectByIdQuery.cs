namespace Ehrms.ProjectManagement.API.Handlers.Project.Queries;

public sealed class GetProjectByIdQuery : IRequest<Models.Project>
{
    public Guid Id { get; set; }
}

internal sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Models.Project>
{
    private readonly ProjectDbContext _dbContext;

    public GetProjectByIdQueryHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Models.Project> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(GetProjectWithId(request.Id));
    }

    private Models.Project GetProjectWithId(Guid id)
    {
        return _dbContext.Projects
            .FirstOrDefault(x => x.Id == id)
            ?? throw new ProjectNotFoundException($"Could not find project with id  '{id}'");
    }
}