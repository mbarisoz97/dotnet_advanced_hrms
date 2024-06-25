using Moq;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ehrms.Contracts.Employee;
using Ehrms.TrainingManagement.API.Consumers.EmployeeEvent;
using Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.EmployeeInfo;

public class EmployeeDeletedEventConsumerTests
{
	private readonly Mock<ILogger<EmployeeDeletedEventConsumer>> _loggerMock = new();

	[Fact]
	public async Task Consume_ExistingEmployee_RemovesFromDatabase()
	{
		var dbContext = TestDbContextFactory.CreateDbContext($"{Guid.NewGuid()}");
		EmployeeDeletedEventConsumer consumer = new(_loggerMock.Object, dbContext);

		var employee = new EmployeeFaker().Generate();
		await dbContext.Employees.AddAsync(employee);
		await dbContext.SaveChangesAsync();

		var employeeDeletedEvent = new EmployeeDeletedEventFaker()
			.WithId(employee.Id)
			.Generate();

		Mock<ConsumeContext<EmployeeDeletedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(employeeDeletedEvent);

		await consumer.Consume(contextMock.Object);

		dbContext.Employees.Should().HaveCount(0);
	}
}