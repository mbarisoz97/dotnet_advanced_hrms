using Moq;
using MassTransit;
using Ehrms.Contracts.Project;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.ProjectEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Project;

public class ProjectCreatedEventConsumerTests
{
	private readonly ProjectCreatedEventConsumer _consumer;
	private readonly Mock<ILogger<ProjectCreatedEventConsumer>> _loggerMock = new();
	private readonly TrainingDbContext _dbContext = TestDbContextFactory.CreateDbContext("ProjectCreatedEventConsumerDb");

	public ProjectCreatedEventConsumerTests()
	{
		var mapper = MapperFactory.CreateWithExistingProfiles();
		_consumer = new(_loggerMock.Object, mapper, _dbContext);
	}

	[Fact]
	public async Task Consume_ValidProject_CreateNewProjectRecord()
	{
		var employees = new EmployeeFaker().Generate(2);
		await _dbContext.AddRangeAsync(employees);
		var skills = new SkillFaker().Generate(2);
		await _dbContext.AddRangeAsync(skills);
		await _dbContext.SaveChangesAsync();

		var projectCreatedEvent = new ProjectCreatedEventFaker()
			.WithEmployees(employees)
			.WithRequiredSkills(skills)
			.Generate();

		Mock<ConsumeContext<ProjectCreatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(projectCreatedEvent);
		await _consumer.Consume(contextMock.Object);

		var project = _dbContext.Projects
			.Include(x => x.Employees)
			.Include(x => x.RequiredSkills)
			.FirstOrDefault(x => x.Id == projectCreatedEvent.Id);

		project.Should().BeEquivalentTo(projectCreatedEvent, opts =>
			opts.Excluding(x => x.Employees)
				.Excluding(x => x.RequiredSkills));

		project?.Employees.Should()
			.Contain(employees);
	
		project?.RequiredSkills.Should()
			.Contain(skills);
	}
}