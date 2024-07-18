using Ehrms.ProjectManagement.API.Consumer.EmployeeEvents;

namespace Ehrms.ProjectManagement.API.UnitTests.Consumers.Employee;

public class EmployeeUpdateConsumerTests
{
	private const string DatabaseName = $"EmployeeUpdateConsumerTestDb";
	private readonly EmployeeUpdatedConsumer _employeeUpdatedConsumer;
	private readonly Mock<ILogger<EmployeeUpdatedConsumer>> _loggerMock = new();

	public EmployeeUpdateConsumerTests()
	{
		var dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase(DatabaseName);
		var mapper = MapperFactory.CreateWithExistingProfiles();

		_employeeUpdatedConsumer = new(_loggerMock.Object, mapper, dbContext);
	}

	[Fact]
	public async Task Consume_ExistingEmployee_UpdatesEmployeeDetails()
	{
		var dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase(DatabaseName);
		var employee = new EmployeeFaker().Generate();
		dbContext.Employees.Add(employee);
		await dbContext.SaveChangesAsync();

		var employeeUpdatedEvent = new EmployeeUpdateEventFaker().Generate();
		employeeUpdatedEvent.Id = employee.Id;

		Mock<ConsumeContext<EmployeeUpdatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(employeeUpdatedEvent);

		await _employeeUpdatedConsumer.Consume(contextMock.Object);

		var updatedEmployee = await dbContext.Employees
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == employee.Id);

		updatedEmployee?.FirstName.Should().Be(employeeUpdatedEvent.FirstName);
		updatedEmployee?.LastName.Should().Be(employeeUpdatedEvent.LastName);
	}
}