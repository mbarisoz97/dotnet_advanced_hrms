using Moq;
using MassTransit;
using Ehrms.Contracts.Employee;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.EmployeeInfo;

public class EmployeeCreatedEventConsumerTests
{
    private readonly EmployeeCreatedEventConsumer _consumer;
    private readonly TrainingDbContext _dbContext;

    public EmployeeCreatedEventConsumerTests()
    {
        _dbContext = TestDbContextFactory.CreateDbContext("EmployeeCreatedEventConsumerDb");
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _consumer = new(mapper, _dbContext);
    }

    [Fact]
    public async Task Consume_ValidEmployeeCreatedEvent_AddedToDatabase()
    {
        var skills = new SkillFaker().Generate(2);
        await _dbContext.Skills.AddRangeAsync(skills);
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);

        await _dbContext.SaveChangesAsync();

        var employeeCreatedEvent = new EmployeeCreatedEventFaker()
            .WithSkills(skills)
            .WithTitle(title)
            .Generate();

        Mock<ConsumeContext<EmployeeCreatedEvent>> contextMock = new();
        contextMock.Setup(x => x.Message)
            .Returns(employeeCreatedEvent);

        await _consumer.Consume(contextMock.Object);
        var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeCreatedEvent.Id);

        employee.Should().BeEquivalentTo(employeeCreatedEvent, opt =>
            opt.Excluding(x => x.Skills)
               .Excluding(p => p.TitleId));
        employee?.Skills.Should().BeEquivalentTo(skills);
    }
}