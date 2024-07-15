using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.Handlers.Project.Queries;

public sealed class GetProjectByIdQuery : IRequest<Database.Models.Project>
{
    public Guid Id { get; set; }
}

internal sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Database.Models.Project>
{
    private readonly ProjectDbContext _dbContext;

    public GetProjectByIdQueryHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Database.Models.Project> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(GetProjectWithId(request.Id));
    }

    private Database.Models.Project GetProjectWithId(Guid id)
    {
        return _dbContext.Projects
            .Include(x=>x.Employments)
            .FirstOrDefault(x => x.Id == id)
            ?? throw new ProjectNotFoundException($"Could not find project with id  '{id}'");
    }
}