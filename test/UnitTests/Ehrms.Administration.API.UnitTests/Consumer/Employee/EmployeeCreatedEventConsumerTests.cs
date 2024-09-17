namespace Ehrms.Administration.API.UnitTests.Consumer.Employee;

public class EmployeeCreatedEventConsumerTests
{
    private readonly AdministrationDbContext _dbContext;
    private readonly EmployeeCreatedEventConsumer _consumer;
    private readonly Mock<ILogger<EmployeeCreatedEventConsumer>> _loggerMock = new();

    public EmployeeCreatedEventConsumerTests()
    {
        _dbContext = DbContextFactory.Create();
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _consumer = new(_dbContext, mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_NullContextMessage_IgnoresEvent()
    {
        EmployeeCreatedEvent employeeCreatedEvent = null!;
        Mock<ConsumeContext<EmployeeCreatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(employeeCreatedEvent!);

        await _consumer.Consume(contextMock.Object);
        _dbContext.Employees.Count().Should().Be(0);
    }

    [Fact]
    public async Task Consume_ValidEventDetails_CreatesNewEmployee()
    {
        var employeeCreatedEvent = new EmployeeCreatedEventFaker().Generate();
        Mock<ConsumeContext<EmployeeCreatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message).Returns(employeeCreatedEvent);
        
        await _consumer.Consume(contextMock.Object);
        
        _dbContext.Employees.Count().Should().Be(1);
        var createdEmployee = _dbContext.Employees.First();
        employeeCreatedEvent.Should().BeEquivalentTo(createdEmployee,
            opt => opt.Excluding(p => p.PaymentCriteria));
    }
}