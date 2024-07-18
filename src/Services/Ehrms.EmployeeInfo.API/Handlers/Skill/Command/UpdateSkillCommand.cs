using Ehrms.Contracts.Skill;
using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

public sealed class UpdateSkillCommand : IRequest<Database.Models.Skill>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
}

internal sealed class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, Database.Models.Skill>
{
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly EmployeeInfoDbContext _dbContext;
	private readonly ILogger<UpdateSkillCommandHandler> _logger;

	public UpdateSkillCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, EmployeeInfoDbContext dbContext, ILogger<UpdateSkillCommandHandler> logger)
	{
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task<Database.Models.Skill> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
	{
		var skill = await UpdateSkillRecord(request, cancellationToken);
		await PublishEvents(skill, cancellationToken);

		return skill;
	}

	private async Task<Database.Models.Skill> UpdateSkillRecord(UpdateSkillCommand request, CancellationToken cancellationToken)
	{
		var skill = await _dbContext.Skills
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new SkillNotFoundException($"Could not find employee skill with id '{request.Id}'");

		if (_dbContext.Skills.Any(x => x.Name == request.Name))
		{
			throw new SkillNameIsInUseException($"'{request.Name}' already in use.");
		}

		_mapper.Map(request, skill);
		_dbContext.Skills.Update(skill);
		await _dbContext.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Updated skill with id : {id} ", skill.Id);

		return skill;
	}

	public async Task PublishEvents(Database.Models.Skill skill, CancellationToken cancellationToken)
	{
		var skillUpdatedEvent = _mapper.Map<SkillUpdatedEvent>(skill);
		await _publishEndpoint.Publish(skillUpdatedEvent, cancellationToken);

		_logger.LogInformation("Published skill updated event with id : {skillId}", skillUpdatedEvent.Id);
	}
}