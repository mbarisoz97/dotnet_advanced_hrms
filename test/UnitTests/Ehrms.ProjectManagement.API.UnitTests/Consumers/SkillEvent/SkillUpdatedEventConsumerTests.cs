using Ehrms.Contracts.Skill;
using Ehrms.ProjectManagement.API.Consumer.SkillEvents;

namespace Ehrms.ProjectManagement.API.UnitTests.Consumers.SkillEvent;

public class SkillUpdatedEventConsumerTests
{
	private readonly ProjectDbContext _dbContext;
	private readonly SkillUpdatedEventConsumer _skillUpdatedEventConsumer;
	private readonly Mock<ILogger<SkillUpdatedEventConsumer>> _loggerMock = new();

	public SkillUpdatedEventConsumerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new SkillMappingProfile());
		}));
		_dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase("SkillCreatedEventTestsDb");
		_skillUpdatedEventConsumer = new(mapper, _loggerMock.Object, _dbContext);
	}

	[Fact]
	public async Task Consume_ExistingSkillId_UpdatesSkill()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		var skillUpdatedEvent = new SkillUpdatedEventFaker()
			.WithId(skill.Id)
			.Generate();

		Mock<ConsumeContext<SkillUpdatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(skillUpdatedEvent);

		await _skillUpdatedEventConsumer.Consume(contextMock.Object);
		skill.Should().BeEquivalentTo(skillUpdatedEvent);
	}

	[Fact]
	public async Task Consume_NonExistingSkillId_IgnoresEvent()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		var skillUpdatedEvent = new SkillUpdatedEventFaker().Generate();

		Mock<ConsumeContext<SkillUpdatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(skillUpdatedEvent);

		await _skillUpdatedEventConsumer.Consume(contextMock.Object);
		skill.Should().NotBeEquivalentTo(skillUpdatedEvent);
	}
}