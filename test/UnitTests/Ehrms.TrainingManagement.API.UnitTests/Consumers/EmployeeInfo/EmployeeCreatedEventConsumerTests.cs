using Moq;
using MassTransit;
using Ehrms.Contracts.Employee;
using Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;
using Ehrms.TrainingManagement.API.Database.Context;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.EmployeeInfo;

public class EmployeeCreatedEventConsumerTests
{
	private readonly EmployeeCreatedEventConsumer _consumer;
	private TrainingDbContext DbContext => TestDbContextFactory.CreateDbContext("EmployeeCreatedEventConsumerDb");

	public EmployeeCreatedEventConsumerTests()
	{
		var mapper = MapperFactory.CreateWithExistingProfiles();
		_consumer = new(mapper, DbContext);
	}

	[Fact]
	public async Task Consume_ValidEmployeeCreatedEvent_AddedToDatabase()
	{
		var employeeCreatedEvent = new EmployeeCreatedEventFaker().Generate();
		Mock<ConsumeContext<EmployeeCreatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(employeeCreatedEvent);

		await _consumer.Consume(contextMock.Object);

		var employee = DbContext.Employees.FirstOrDefault(x => x.Id == employeeCreatedEvent.Id);

		employee.Should().BeEquivalentTo(employeeCreatedEvent);
	}
}