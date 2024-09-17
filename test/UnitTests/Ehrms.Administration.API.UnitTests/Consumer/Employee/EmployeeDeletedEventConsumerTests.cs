namespace Ehrms.Administration.API.UnitTests.Consumer.Employee;

public class EmployeeDeletedEventConsumerTests
{
    private readonly AdministrationDbContext _dbContext;
    private readonly EmployeeDeletedEventConsumer _consumer;
    private readonly Mock<ILogger<EmployeeDeletedEventConsumer>> _loggerMock = new();

    public EmployeeDeletedEventConsumerTests()
    {
        _dbContext = DbContextFactory.Create();
        _consumer = new(_dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_ExistingEmployeeId_RemovesEmployee()
    {
        var employee = new EmployeeFaker().Generate();
        await _dbContext.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        var employeeDeletedEvent = new EmployeeDeletedEventFaker()
            .WithId(employee.Id)
            .Generate();

        Mock<ConsumeContext<EmployeeDeletedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeDeletedEvent);

        await _consumer.Consume(contextMock.Object);

        _dbContext.Employees.Count().Should().Be(0);
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
        EmployeeDeletedEvent employeeDeletedEvent = null!;
        Mock<ConsumeContext<EmployeeDeletedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeDeletedEvent!);

        await _consumer.Consume(contextMock.Object);
        _dbContext.Employees.Count().Should().Be(0);
    }
}