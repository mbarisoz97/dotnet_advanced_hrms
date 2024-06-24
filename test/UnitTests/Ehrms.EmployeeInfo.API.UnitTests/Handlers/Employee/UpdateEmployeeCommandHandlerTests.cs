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

		var command = new UpdateEmployeeCommandFaker().Generate();
		command.Id = employee.Id;

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
		var employee = new EmployeeFaker().Generate();
		await dbContext.Employees.AddAsync(employee);
		await dbContext.SaveChangesAsync();

		var command = new UpdateEmployeeCommandFaker().Generate();
		command.Id = employee.Id;
		var updateEmployee = await _handler.Handle(command, default);

		updateEmployee?.Id.Should().Be(command.Id);
		updateEmployee?.FirstName.Should().Be(command.FirstName);
		updateEmployee?.LastName.Should().Be(command.LastName);
		updateEmployee?.DateOfBirth.Should().Be(command.DateOfBirth);
		updateEmployee?.Qualification.Should().Be(command.Qualification);
	}
}