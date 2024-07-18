using Ehrms.Contracts.Skill;
using Ehrms.ProjectManagement.API.Consumer.SkillEvents;

namespace Ehrms.ProjectManagement.API.UnitTests.Consumers.SkillEvent;

public class SkillCreatedEventConsumerTests
{
	private readonly SkillCreatedEventConsumer _skillCreatedEventConsumer;
	private readonly Mock<ILogger<SkillCreatedEventConsumer>> _loggerMock = new();

	private ProjectDbContext DbContext => CustomDbContextFactory.CreateWithInMemoryDatabase("SkillCreatedEventTestsDb");

	public SkillCreatedEventConsumerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new SkillMappingProfile());
		}));

		_skillCreatedEventConsumer = new(_loggerMock.Object, mapper, DbContext);
	}

	[Fact]
	public async Task Consume_NonExistingSkillId_SavesSkillRecord()
	{
		var skillCreatedEvent = new SkillCreatedEventFaker().Generate();

		Mock<ConsumeContext<SkillCreatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(skillCreatedEvent);

		await _skillCreatedEventConsumer.Consume(contextMock.Object);

		var createdSkill = await DbContext.Skills
			.FirstOrDefaultAsync(x => x.Id == skillCreatedEvent.Id, default);

		createdSkill.Should().BeEquivalentTo(skillCreatedEvent);
	}
}