using Microsoft.Extensions.Logging;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;
using Ehrms.Contracts.Skill;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Skill;

public class DeletekillCommandHandlerTests
{
	private readonly DeleteSkillCommandHandler _handler;

	private readonly EmployeeInfoDbContext _dbContext;
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly Mock<ILogger<DeleteSkillCommandHandler>> _loggerMock = new();

	public DeletekillCommandHandlerTests()
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
	public async Task Handle_NonExistingEmployeeId_ThrowsSkillNotFoundException()
	{
		var command = new DeleteCommandFaker().Generate();

		await Assert.ThrowsAsync<SkillNotFoundException>(async () =>
		{
			await _handler.Handle(command, default);
		});
	}

	[Fact]
	public async Task Handle_ExistingEmployeeId_RemovesSkillRecord()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill, default);
		await _dbContext.SaveChangesAsync(default);

		var command = new DeleteCommandFaker()
			.WithId(skill.Id)
			.Generate();

		await _handler.Handle(command, default);
		_dbContext.Skills.Should().HaveCount(0);
	}

	[Fact]
	public async Task Handle_SuccessfullDelete_PublishedSkillDeletedEvent()
	{
		var skill = new SkillFaker().Generate();
		await _dbContext.AddAsync(skill, default);
		await _dbContext.SaveChangesAsync(default);

		var command = new DeleteCommandFaker()
			.WithId(skill.Id)
			.Generate();

		await _handler.Handle(command, default);

		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<SkillDeletedEvent>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
	}
}