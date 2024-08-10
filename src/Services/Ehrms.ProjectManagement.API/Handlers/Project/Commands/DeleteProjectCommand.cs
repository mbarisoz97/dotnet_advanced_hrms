using Ehrms.Contracts.Project;

namespace Ehrms.ProjectManagement.API.Handlers.Project.Commands;

internal sealed class DeleteProjectCommand : IRequest { public Guid Id { get; set; } }

internal sealed class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
	private readonly ProjectDbContext _dbContext;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly IMapper _mapper;

	public DeleteProjectCommandHandler(ProjectDbContext dbContext,
		IPublishEndpoint publishEndpoint, 
		IMapper mapper)
	{
		_dbContext = dbContext;
		_publishEndpoint = publishEndpoint;
		_mapper = mapper;
	}

	public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
	{
		var project = _dbContext.Projects
			.FirstOrDefault(x => x.Id == request.Id)
			?? throw new ProjectNotFoundException($"Could not find project with id '{request.Id}'");

		_dbContext.Remove(project);
		await _dbContext.SaveChangesAsync(cancellationToken);

		var projectDeletedEvent = _mapper.Map<ProjectDeletedEvent>(project);
		await _publishEndpoint.Publish(projectDeletedEvent, cancellationToken);
	}
}