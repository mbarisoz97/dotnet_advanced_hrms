using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.API.UnitTests.Handlers.Employee;

public class CreateEmployeeCommandHandlerTests
{
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
    private readonly EmployeeInfoDbContext _employeeInfoDbContext;
    private readonly CreateEmployeeCommandHandler _handler;

    public CreateEmployeeCommandHandlerTests()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EmployeeMappingProfiles());
        }));

        _employeeInfoDbContext = new(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _handler = new(_publishEndpointMock.Object, mapper, _employeeInfoDbContext);
    }

    [Fact]
    public async Task Handle_ValidEmployeeInformation_CreatesNewEmployee()
    {
        var title = new TitleFaker().Generate();
        await _employeeInfoDbContext.AddAsync(title);
        await _employeeInfoDbContext.SaveChangesAsync();

        var command = new CreateEmployeeCommandFaker()
            .WithTitleId(title.Id)
            .Generate();

        var commandResult = await _handler.Handle(command, default);
        var employee = commandResult.Match<Database.Models.Employee?>(e => e, _ => null);

        employee.Should().BeEquivalentTo(command, opt =>
            opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Handle_InvalidTitleId_ReturnsResultWithTitleNotFoundException()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        var commandResult = await _handler.Handle(command, default);

        var exceptionInResult = commandResult.Match<Exception?>(_ => null, f => f);
        exceptionInResult.Should().BeOfType<TitleNotFoundException>();
    }

    [Fact]
    public async Task Handle_SuccessfullCreatedEmployee_PublishedCreatedEvent()
    {
        var title = new TitleFaker().Generate();
        await _employeeInfoDbContext.AddAsync(title);
        await _employeeInfoDbContext.SaveChangesAsync();

        var command = new CreateEmployeeCommandFaker()
            .WithTitleId(title.Id)
            .Generate();

        await _handler.Handle(command, default);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<EmployeeCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}