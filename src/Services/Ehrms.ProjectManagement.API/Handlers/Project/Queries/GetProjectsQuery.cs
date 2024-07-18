namespace Ehrms.ProjectManagement.API.Handlers.Project.Queries;

internal sealed record GetProjectsQuery : IRequest<IQueryable<Database.Models.Project>> { }

internal sealed class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IQueryable<Database.Models.Project>>
{
    private readonly ProjectDbContext _dbContext;

    public GetProjectsQueryHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Database.Models.Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbContext.Projects);
    }
}