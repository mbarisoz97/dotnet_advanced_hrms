using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class CreateProjectCommandHandlerTests
{
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

		_handler = new(mapper, _projectDbContext);
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
}