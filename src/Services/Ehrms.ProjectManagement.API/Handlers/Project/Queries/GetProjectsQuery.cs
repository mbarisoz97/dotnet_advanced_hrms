namespace Ehrms.ProjectManagement.API.Handlers.Project.Queries;

internal sealed record GetProjectsQuery : IRequest<IQueryable<Models.Project>> { }

internal sealed class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IQueryable<Models.Project>>
{
    private readonly ProjectDbContext _dbContext;

    public GetProjectsQueryHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Models.Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.Projects);
    }
}