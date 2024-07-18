using Ehrms.Contracts.Skill;
using Microsoft.Extensions.Logging;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Skill;

public class CreateSkillCommandHandlerTests
{
	private readonly CreateSkillCommandHandler _handler;

	private readonly EmployeeInfoDbContext _dbContext;
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly Mock<ILogger<CreateSkillCommandHandler>> _loggerMock = new();

	public CreateSkillCommandHandlerTests()
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
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		var command = new CreateSkillCommandFaker()
			.WithName(skill.Name)
			.Generate();

		await Assert.ThrowsAsync<SkillNameIsInUseException>(async () =>
		{
			await _handler.Handle(command, default);
		});
	}

	[Fact]
	public async Task Handle_SuccessfullCreatedEvent_PublishedSkillCreatedEvent()
	{
		var command = new CreateSkillCommandFaker();
		await _handler.Handle(command, default);

		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<SkillCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
	}
}
