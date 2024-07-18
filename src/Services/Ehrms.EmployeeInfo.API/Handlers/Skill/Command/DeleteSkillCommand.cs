using Ehrms.Contracts.Skill;
using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

internal sealed class DeleteSkillCommand : IRequest
{
	public Guid Id { get; set; }
}

internal sealed class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand>
{
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly ILogger<DeleteSkillCommandHandler> _logger;

	private readonly EmployeeInfoDbContext _dbContext;

	public DeleteSkillCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, EmployeeInfoDbContext dbContext, ILogger<DeleteSkillCommandHandler> logger)
	{
		_publishEndpoint = publishEndpoint;
		_mapper = mapper;
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
	{
		await DeleteSkill(request, cancellationToken);
		await PublishMessageQueueEvents(request, cancellationToken);
	}

	private async Task PublishMessageQueueEvents(DeleteSkillCommand request, CancellationToken cancellationToken)
	{
		var skillDeletedEvent = _mapper.Map<SkillDeletedEvent>(request);
		await _publishEndpoint.Publish(skillDeletedEvent, cancellationToken);

		_logger.LogInformation("Published skill deleted event with id : {id}", request.Id);
	}

	private async Task DeleteSkill(DeleteSkillCommand request, CancellationToken cancellationToken)
	{
		var skill = await _dbContext.Skills
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new SkillNotFoundException($"Could not find employee skill with id '{request.Id}'");

		_dbContext.Remove(skill);
		await _dbContext.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Deleted skill with id : {id}", request.Id);
	}
}