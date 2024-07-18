using Microsoft.Extensions.Logging;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;
using Ehrms.Contracts.Skill;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Skill;

public class UpdateSkillCommandHandlerTests
{
	private readonly UpdateSkillCommandHandler _handler;

	private readonly EmployeeInfoDbContext _dbContext;
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly Mock<ILogger<UpdateSkillCommandHandler>> _loggerMock = new();

	public UpdateSkillCommandHandlerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new SkillMappingProfiles());
		}));

		_dbContext = new(new DbContextOptionsBuilder()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options);

		_handler = new(mapper, _publishEndpointMock.Object, _dbContext, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ExistingSkillName_ThrowsSkillNameIsInUseException()
	{
		var skills = new SkillFaker().Generate(2);
		await _dbContext.AddRangeAsync(skills);
		await _dbContext.SaveChangesAsync();

		var command = new UpdateSkillCommandFaker()
			.WithId(skills[1].Id)
			.WithName(skills[0].Name)
			.Generate();

		await Assert.ThrowsAsync<SkillNameIsInUseException>(async () =>
		{
			await _handler.Handle(command, default);
		});
	}

	[Fact]
	public async Task Handle_SuccessfullCreatedEvent_PublishedSkillCreatedEvent()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		var command = new UpdateSkillCommandFaker()
			.WithId(skill.Id)
			.Generate();

		await _handler.Handle(command, default);
		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<SkillUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
	}
}