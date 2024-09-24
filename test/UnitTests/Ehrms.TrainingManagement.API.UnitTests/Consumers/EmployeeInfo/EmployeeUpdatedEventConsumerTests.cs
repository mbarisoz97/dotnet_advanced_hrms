using Moq;
using MassTransit;
using Ehrms.Contracts.Employee;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;
using Microsoft.Extensions.Logging;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.EmployeeInfo;

public class EmployeeUpdatedEventConsumerTests
{
	private readonly Mock<ILogger<EmployeeUpdatedEventConsumer>> _loggerMock = new();
	private readonly IMapper _mapper = MapperFactory.CreateWithExistingProfiles();

	[Fact]
	public async Task Consume_ExistingEmployee_UpdatesEmployeeRecord()
	{
		var dbContext = TestDbContextFactory.CreateDbContext($"{Guid.NewGuid()}");
		EmployeeUpdatedEventConsumer consumer = new(_mapper, dbContext, _loggerMock.Object);

        var titleBeforeUpdate = new TitleFaker().Generate();
        var titleAfterUpdate = new TitleFaker().Generate();
        await dbContext.AddRangeAsync(titleAfterUpdate, titleAfterUpdate);

        var employee = new EmployeeFaker()
			.WithTitle(titleBeforeUpdate)
			.Generate();
		await dbContext.Employees.AddAsync(employee);

		await dbContext.SaveChangesAsync();

		var employeeUpdatedEvent = new EmployeeUpdatedEventFaker()
			.WithId(employee.Id)
			.WithTitle(titleAfterUpdate)
			.Generate();

		Mock<ConsumeContext<EmployeeUpdatedEvent>> contextMock = new();
		contextMock.Setup(x => x.Message)
			.Returns(employeeUpdatedEvent);

		await consumer.Consume(contextMock.Object);

		employee.Should().BeEquivalentTo(employeeUpdatedEvent, opt => 
			opt.Excluding(p => p.TitleId));
	}
}