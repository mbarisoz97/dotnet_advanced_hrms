using Ehrms.Contracts.Project;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class CreateProjectCommandHandlerTests
{
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly CreateProjectCommandHandler _handler;
	private readonly ProjectDbContext _projectDbContext;

	public CreateProjectCommandHandlerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new ProjectMappingProfile());
		}));

		_projectDbContext = new(new DbContextOptionsBuilder()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options);

		_handler = new(mapper, _publishEndpointMock.Object, _projectDbContext);
	}

	[Fact]
	public async Task Handle_WithNoEmployees_SavesProject()
	{
		CreateProjectCommand command = new CreateProjectCommandFaker().Generate();
		await _handler.Handle(command, default);
		var project = _projectDbContext.Projects.FirstOrDefault();

		_projectDbContext.Projects.Count().Should().Be(1);

		project?.Id.Should().NotBe(Guid.Empty);
		project?.Name.Should().Be(command.Name);
		project?.Description.Should().Be(command.Description);
	}

	[Fact]
	public async Task Handle_WithMultipleEmployees_SavesProject()
	{
		var employeeCollection = new EmployeeFaker().Generate(2);

		await _projectDbContext.Employees
			.AddRangeAsync(employeeCollection);
		await _projectDbContext.SaveChangesAsync();

		CreateProjectCommand command = new CreateProjectCommandFaker()
			.WithEmployees(employeeCollection)
			.Generate();

		var project = await _handler.Handle(command, default);

		_projectDbContext.Projects.Count().Should().Be(1);
		project?.Id.Should().NotBe(Guid.Empty);
		project?.Name.Should().Be(command.Name);
		project?.Description.Should().Be(command.Description);
		project?.Employments?.Should().HaveCount(employeeCollection.Count);
	}

	[Fact]
	public async Task Handle_WithRequiredSkillds_SavesProject()
	{
		var skills = new SkillFaker().Generate(3);
		await _projectDbContext.AddRangeAsync(skills);
		var employees = new EmployeeFaker().Generate(4);
		await _projectDbContext.AddRangeAsync(employees);
		await _projectDbContext.SaveChangesAsync();

		CreateProjectCommand command = new CreateProjectCommandFaker()
			.WithRequiredSkills(skills)
			.WithEmployees(employees)
			.Generate();

		var project = await _handler.Handle(command, default);

		project.Should().BeEquivalentTo(command, options =>
			options.Excluding(x => x.Employees)
				   .Excluding(x => x.RequiredSkills));

		project.Employments.Should().HaveCount(employees.Count);
		project.RequiredProjectSkills.Should().HaveCount(skills.Count);
	}

	[Fact]
	public async Task Handle_SuccessfullyCreatedProject_PublishedProjectCreatedEvent()
	{
		ProjectCreatedEvent? projectCreatedEvent = null;
		_publishEndpointMock.Setup(x => x.Publish(It.IsAny<ProjectCreatedEvent>(), It.IsAny<CancellationToken>()))
			.Callback((ProjectCreatedEvent @event, CancellationToken token) =>
			{
				projectCreatedEvent = @event;
			});

		CreateProjectCommand command = new CreateProjectCommandFaker().Generate();
		var project = await _handler.Handle(command, default);

		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<ProjectCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);

		projectCreatedEvent.Should().NotBeNull();
		projectCreatedEvent?.Id.Should().Be(project.Id);
		projectCreatedEvent.Should().BeEquivalentTo(command, opts => opts.Excluding(x=>x.Description));
	}
}