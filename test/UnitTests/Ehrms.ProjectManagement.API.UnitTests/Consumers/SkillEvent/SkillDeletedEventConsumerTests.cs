using Ehrms.Contracts.Skill;
using Ehrms.ProjectManagement.API.Consumer.SkillEvents;

namespace Ehrms.ProjectManagement.API.UnitTests.Consumers.SkillEvent;

public class SkillDeletedEventConsumerTests
{
	private readonly ProjectDbContext _dbContext;
	private readonly SkillDeletedEventConsumer _skillDeletedEventConsumer;
	private readonly Mock<ILogger<SkillDeletedEventConsumer>> _loggerMock = new();

	public SkillDeletedEventConsumerTests()
	{
		_dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase("SkillCreatedEventTestsDb");
		_skillDeletedEventConsumer = new(_loggerMock.Object, _dbContext);
	}

	[Fact]
	public async Task Consume_ExistingSkillId_RemovesSkill()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill);
		await _dbContext.SaveChangesAsync();

		var skillDeletedEvent = new SkillDeletedEventFaker()
			.WithId(skill.Id)
			.Generate();

		Mock<ConsumeContext<SkillDeletedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(skillDeletedEvent);

		await _skillDeletedEventConsumer.Consume(contextMock.Object);

		var skills = _dbContext.Skills.Where(x => x.Id == skill.Id);
		skills.Should().HaveCount(0);
	}
}
