using Ehrms.ProjectManagement.API.Database.Context;
using Ehrms.ProjectManagement.API.Exceptions;

namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

internal sealed class DeleteProjectCommand : IRequest { public Guid Id { get; set; } }

internal sealed class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly ProjectDbContext _dbContext;

    public DeleteProjectCommandHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = _dbContext.Projects
            .FirstOrDefault(x => x.Id == request.Id)
            ?? throw new ProjectNotFoundException($"Could not find project with id '{request.Id}'");
        
        _dbContext.Remove(project);
        await _dbContext.SaveChangesAsync();    
    }
}