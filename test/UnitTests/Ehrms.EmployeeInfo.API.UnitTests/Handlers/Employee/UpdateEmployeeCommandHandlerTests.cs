namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Employee;

public class UpdateEmployeeCommandHandlerTests
{
	private const string DatabaseName = " UpdateEmployeeCommandHandlerTestsDb";
	private readonly UpdateEmployeeCommandHandler _handler;
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

	public UpdateEmployeeCommandHandlerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new EmployeeMappingProfiles());
		}));

		var employeeInfoDbContext = DbContextFactory.Create(DatabaseName);
		_handler = new(employeeInfoDbContext, _publishEndpointMock.Object, mapper);
	}

	[Fact]
	public async Task Handle_SuccesfullyUpdateEmployee_PublishesEmployeeUpdatedEvent()
	{
		var dbContext = DbContextFactory.Create(DatabaseName);
		var employee = new EmployeeFaker().Generate();
		await dbContext.Employees.AddAsync(employee);
		await dbContext.SaveChangesAsync();

		var command = new UpdateEmployeeCommandFaker()
			.WithId(employee.Id)
			.Generate();

		EmployeeUpdatedEvent? employeeUpdatedEvent = null;
		_publishEndpointMock.Setup(x => x.Publish(It.IsAny<EmployeeUpdatedEvent>(), It.IsAny<CancellationToken>()))
			.Callback((EmployeeUpdatedEvent @event, CancellationToken token) =>
			{
				employeeUpdatedEvent = @event;
			});

		await _handler.Handle(command, default);

		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<EmployeeUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);

		employeeUpdatedEvent.Should().NotBeNull();
		employeeUpdatedEvent?.Id.Should().Be(command.Id);
		employeeUpdatedEvent?.FirstName.Should().Be(command.FirstName);
		employeeUpdatedEvent?.LastName.Should().Be(command.LastName);
	}

	[Fact]
	public async Task Handle_NonExistingEmployee_ThrowsEmployeeNotFoundException()
	{
		var command = new UpdateEmployeeCommandFaker().Generate();
		await Assert.ThrowsAsync<EmployeeNotFoundException>(async () =>
		{
			await _handler.Handle(command, default);
		});
	}

	[Fact]
	public async Task Handle_ExistingEmployee_UpdatesEmployeeRecord()
	{
		var dbContext = DbContextFactory.Create(DatabaseName);
		var skills = new SkillFaker().Generate(2);
		await dbContext.AddRangeAsync(skills);

		var employee = new EmployeeFaker().Generate();
		await dbContext.Employees.AddAsync(employee);
		await dbContext.SaveChangesAsync();

		var command = new UpdateEmployeeCommandFaker()
			.WithId(employee.Id)
			.WithSkills(skills.Select(x=>x.Id).ToArray())
			.Generate();

		var updatedEmployee = await _handler.Handle(command, default);

		updatedEmployee?.Id.Should().Be(command.Id);
		updatedEmployee?.FirstName.Should().Be(command.FirstName);
		updatedEmployee?.LastName.Should().Be(command.LastName);
		updatedEmployee?.DateOfBirth.Should().Be(command.DateOfBirth);
		updatedEmployee?.Qualification.Should().Be(command.Qualification);
		updatedEmployee?.Skills.Should().HaveCount(command.Skills.Count);
	}

	[Fact]
	public async Task Handle_RemovedEmployeeSkills_UpdatesEmployeeSuccessfully()
	{
		var dbContext = DbContextFactory.Create(DatabaseName);
		var initialSkills = new SkillFaker().Generate(2);
		await dbContext.AddRangeAsync(initialSkills);

		var employee = new EmployeeFaker()
			.WithSkills(initialSkills)
			.Generate();

		await dbContext.Employees.AddAsync(employee);
		await dbContext.SaveChangesAsync();

		var updatedSkills = Array.Empty<Guid>();
		var command = new UpdateEmployeeCommandFaker()
			.WithId(employee.Id)
			.WithSkills(updatedSkills)
			.Generate();

		var updatedEmployee = await _handler.Handle(command, default);

		updatedEmployee?.Id.Should().Be(command.Id);
		updatedEmployee?.FirstName.Should().Be(command.FirstName);
		updatedEmployee?.LastName.Should().Be(command.LastName);
		updatedEmployee?.DateOfBirth.Should().Be(command.DateOfBirth);
		updatedEmployee?.Qualification.Should().Be(command.Qualification);
		updatedEmployee?.Skills.Should().HaveCount(command.Skills.Count);
	}
}