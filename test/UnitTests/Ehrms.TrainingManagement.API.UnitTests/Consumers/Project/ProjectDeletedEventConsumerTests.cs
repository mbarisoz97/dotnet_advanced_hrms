using Moq;
using MassTransit;
using Ehrms.Contracts.Project;
using Microsoft.Extensions.Logging;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.ProjectEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Project;

public class ProjectDeletedEventConsumerTests : IAsyncLifetime
{
	private readonly ProjectDeletedEventConsumer _consumer;
	private readonly Mock<ILogger<ProjectDeletedEventConsumer>> _loggerMock = new();
	private readonly TrainingDbContext _dbContext = TestDbContextFactory.CreateDbContext("ProjectDeletedEventConsumerDb");

	public ProjectDeletedEventConsumerTests()
	{
		_consumer = new(_dbContext, _loggerMock.Object);
	}

	[Fact]
	public async Task Consume_ExistingProjectId_RemovesProjectRecord()
	{
		var project = new ProjectFaker().Generate();
		await _dbContext.AddAsync(project);
		await _dbContext.SaveChangesAsync();

		var projectDeletedEvent = new ProjectDeletedEventFaker()
			.WithId(project.Id)
			.Generate();

		Mock<ConsumeContext<ProjectDeletedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(projectDeletedEvent);

		await _consumer.Consume(contextMock.Object);

		var returnedProject = _dbContext.Projects.FirstOrDefault(x => x.Id == projectDeletedEvent.Id);
		returnedProject.Should().BeNull();
	}

	[Fact]
	public async Task Consume_NonExistingProjectId_IgnoresDeleteEvent()
	{
		var project = new ProjectFaker().Generate();
		await _dbContext.AddAsync(project);
		await _dbContext.SaveChangesAsync();

		var projectDeletedEvent = new ProjectDeletedEventFaker().Generate();

		Mock<ConsumeContext<ProjectDeletedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(projectDeletedEvent);

		await _consumer.Consume(contextMock.Object);

		_dbContext.Projects.Should().HaveCount(1);
	}

	[Fact]
	public async Task Consume_NullEvent_IgnoresDeleteEvent()
	{
		var project = new ProjectFaker().Generate();
		await _dbContext.AddAsync(project);
		await _dbContext.SaveChangesAsync();

		ProjectDeletedEvent projectDeletedEvent = null!;
		Mock<ConsumeContext<ProjectDeletedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(projectDeletedEvent);

		await _consumer.Consume(contextMock.Object);

		_dbContext.Projects.Should().HaveCount(1);
	}

	public async Task DisposeAsync()
	{
		await _dbContext.Database.EnsureDeletedAsync();
	}

	public async Task InitializeAsync()
	{
		await _dbContext.Database.EnsureCreatedAsync();
	}
}