namespace Ehrms.Administration.API.UnitTests.Consumer.Employee;

public class EmployeeUpdateEventConsumerTests
{
    private readonly AdministrationDbContext _dbContext;
    private readonly EmployeeUpdatedEventConsumer _consumer;
    private readonly Mock<ILogger<EmployeeUpdatedEventConsumer>> _loggerMock = new();

    public EmployeeUpdateEventConsumerTests()
    {
        _dbContext = DbContextFactory.Create();
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _consumer = new EmployeeUpdatedEventConsumer(_dbContext, mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_NonExistingEmployeeId_IgnoresEvent()
    {
        var employeeUpdateEvent = new EmployeeUpdatedEventFaker().Generate();

        Mock<ConsumeContext<EmployeeUpdatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeUpdateEvent);

        await _consumer.Consume(contextMock.Object);
        _dbContext.Employees.Count().Should().Be(0);
    }

    [Fact]
    public async Task Consume_ExistingEmployeeId_UpdatesEmployeeDetails()
    {
        var employee = new EmployeeFaker().Generate();
        await _dbContext.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        var employeeUpdatedEvent = new EmployeeUpdatedEventFaker()
            .WithId(employee.Id)
            .WithFirstName("MyFirstName")
            .WithLastName("MyLastName")
            .Generate();

        Mock<ConsumeContext<EmployeeUpdatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeUpdatedEvent);

        await _consumer.Consume(contextMock.Object);
        employeeUpdatedEvent.Should().BeEquivalentTo(employee, opt =>
            opt.Excluding(p => p.PaymentCriteria));
    }

    [Fact]
    public async Task Consume_NullConsumeContext_IgnoredEvent()
    {
        await _consumer.Consume(null!);
        _dbContext.Employees.Count().Should().Be(0);
    }

    [Fact]
    public async Task Consume_NullContextMessage_IgnoredEvent()
    {
        EmployeeUpdatedEvent employeeUpdatedEvent = null!;
        Mock<ConsumeContext<EmployeeUpdatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeUpdatedEvent!);

        await _consumer.Consume(contextMock.Object);
        _dbContext.Employees.Count().Should().Be(0);
    }
}