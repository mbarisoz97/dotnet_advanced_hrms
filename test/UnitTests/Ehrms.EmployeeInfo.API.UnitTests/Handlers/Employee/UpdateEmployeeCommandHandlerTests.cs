namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Employee;

public class UpdateEmployeeCommandHandlerTests
{
    private readonly EmployeeInfoDbContext _dbContext;
    private readonly UpdateEmployeeCommandHandler _handler;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();

    public UpdateEmployeeCommandHandlerTests()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EmployeeMappingProfiles());
        }));

        _dbContext = DbContextFactory.Create(Guid.NewGuid().ToString());
        _handler = new(_dbContext, _publishEndpointMock.Object, mapper);
    }

    [Fact]
    public async Task Handle_SuccesfullyUpdateEmployee_PublishesEmployeeUpdatedEvent()
    {
        var employee = new EmployeeFaker().Generate();
        await _dbContext.Employees.AddAsync(employee);
        
        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);
        
        await _dbContext.SaveChangesAsync();
        var command = new UpdateEmployeeCommandFaker()
            .WithId(employee.Id)
            .WithTitleId(title.Id)
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
    public async Task Handle_NonExistingEmployee_ReturnsResultWithEmployeeNotFoundException()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);
        var expectionInResult = commandResult.Match<Exception?>(_ => null, f => f);
        
        expectionInResult.Should().BeOfType<EmployeeNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingEmployee_UpdatesEmployeeRecord()
    {
        var skills = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(skills);

        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);

        var employee = new EmployeeFaker().Generate();
        await _dbContext.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateEmployeeCommandFaker()
            .WithId(employee.Id)
            .WithSkills(skills)
            .WithTitleId(title.Id)
            .Generate();

        var commandResult = await _handler.Handle(command, default);
        var employeeInResult = commandResult.Match<Database.Models.Employee?>(s => s, _ => null);

        employeeInResult.Should().BeEquivalentTo(employee, opt => opt
            .Excluding(p => p.Skills)
            .Excluding(p => p.Title));

        employeeInResult?.Title.Should().BeEquivalentTo(title);
        employeeInResult?.Skills.Should().BeEquivalentTo(skills);
    }

    [Fact]
    public async Task Handle_RemovedEmployeeSkills_UpdatesEmployeeSuccessfully()
    {
        var initialSkills = new SkillFaker().Generate(2);
        await _dbContext.AddRangeAsync(initialSkills);

        var title = new TitleFaker().Generate();
        await _dbContext.AddAsync(title);

        var employee = new EmployeeFaker()
            .WithSkills(initialSkills)
            .WithTitle(title)
            .Generate();

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        var updatedSkills = Array.Empty<Guid>();
        var command = new UpdateEmployeeCommandFaker()
            .WithId(employee.Id)
            .WithTitleId(title.Id)
            .WithSkills(updatedSkills)
            .Generate();

        var commandResult = await _handler.Handle(command, default);
        var employeeInResult = commandResult.Match<Database.Models.Employee?>(s => s, _ => null);

        employeeInResult.Should().BeEquivalentTo(employee, opt => opt
            .Excluding(p => p.Skills)
            .Excluding(p => p.Title));
        
        employeeInResult?.Skills.Should().BeEquivalentTo(updatedSkills);
        employeeInResult?.Title.Should().BeEquivalentTo(title);
    }
}