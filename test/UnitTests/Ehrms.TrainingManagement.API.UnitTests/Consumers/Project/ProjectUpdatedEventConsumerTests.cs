using Moq;
using MassTransit;
using Ehrms.Contracts.Project;
using Microsoft.Extensions.Logging;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.ProjectEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Project;

public class ProjectUpdatedEventConsumerTests
{
	private readonly ProjectUpdatedEventConsumer _consumer;
	private readonly Mock<ILogger<ProjectUpdatedEventConsumer>> _loggerMock = new();
	private readonly TrainingDbContext _dbContext = TestDbContextFactory.CreateDbContext("ProjectUpdatedEventConsumerDb");

	public ProjectUpdatedEventConsumerTests()
	{
		var mapper = MapperFactory.CreateWithExistingProfiles();
		_consumer = new(_loggerMock.Object, mapper, _dbContext);
	}

	[Fact]
	public async Task Consume_ValidProjectDetails_UpdatesNewProjectRecord()
	{
		var employees = new EmployeeFaker().Generate(2);
		await _dbContext.AddRangeAsync(employees);
		var skills = new SkillFaker().Generate(2);
		await _dbContext.AddRangeAsync(skills);
		var project = new ProjectFaker().Generate();
		await _dbContext.AddRangeAsync(project);
		await _dbContext.SaveChangesAsync();

		var projectCreatedEvent = new ProjectUpdatedEventFaker()
			.WithId(project.Id)
			.WithEmployees(employees)
			.WithRequiredSkills(skills)
			.Generate();

		Mock<ConsumeContext<ProjectUpdatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(projectCreatedEvent);
		await _consumer.Consume(contextMock.Object);

		project.Should().BeEquivalentTo(projectCreatedEvent, opts =>
			opts.Excluding(x => x.Employees)
				.Excluding(x => x.RequiredSkills));

		project?.Employees.Should().Contain(employees);
		project?.RequiredSkills.Should().Contain(skills);
	}
}