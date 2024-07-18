using Ehrms.Contracts.Skill;
using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

public sealed class CreateSkillCommand : IRequest<Database.Models.Skill>
{
	public string Name { get; set; } = string.Empty;
}

internal sealed class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, Database.Models.Skill>
{
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly EmployeeInfoDbContext _dbContext;
	private readonly ILogger<CreateSkillCommandHandler> _logger;

	public CreateSkillCommandHandler(IMapper mapper,
		IPublishEndpoint publishEndpoint,
		EmployeeInfoDbContext dbContext,
		ILogger<CreateSkillCommandHandler> logger)
	{
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
		_dbContext = dbContext;
		_logger = logger;
	}

	public async Task<Database.Models.Skill> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
	{
		var skill = await CreateNewSkill(request, cancellationToken);
		await PublishQueueEvents(skill, cancellationToken);

		return skill;
	}

	private async Task<Database.Models.Skill> CreateNewSkill(CreateSkillCommand request, CancellationToken cancellationToken)
	{
		bool isSkillNameInUse = _dbContext.Skills.Any(x => x.Name == request.Name);
		if (isSkillNameInUse)
		{
			throw new SkillNameIsInUseException($"'{request.Name}' already in use.");
		}

		var skill = _mapper.Map<Database.Models.Skill>(request);
		_dbContext.Skills.Add(skill);
		await _dbContext.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("New skill created with id : {id}", skill.Id);

		return skill;
	}

	private async Task PublishQueueEvents(Database.Models.Skill skill, CancellationToken cancellationToken)
	{
		var skillCreatedEvent = _mapper.Map<SkillCreatedEvent>(skill);
		await _publishEndpoint.Publish(skillCreatedEvent, cancellationToken);
		_logger.LogInformation("Published skill created event with id : {id}", skill.Id);
	}
}