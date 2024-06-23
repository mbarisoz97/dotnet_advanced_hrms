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
        var command = new CreateEmployeeCommandFaker().Generate();
        var employee = await _handler.Handle(command, default);

        employee.Id.Should().NotBe(Guid.Empty);
        employee.FirstName.Should().Be(command.FirstName);
        employee.LastName.Should().Be(command.LastName);
        employee.Qualification.Should().Be(command.Qualification);
        employee.DateOfBirth.Should().Be(command.DateOfBirth);
    }

    [Fact]
    public async Task Handle_SuccessfullCreatedEmployee_PublishedCreatedEvent()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        var employee = await _handler.Handle(command, default);

        _publishEndpointMock.Verify(x=> x.Publish(It.IsAny<EmployeeCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}