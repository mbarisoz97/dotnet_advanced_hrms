namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training;

public class CreateTrainingCommandHandlerTests
{
    private const string DatabaseName = "CreateTrainingCommandHandlerTestDb";
    private readonly CreateTrainingCommandHandler _handler;

    public CreateTrainingCommandHandlerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        var dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase(DatabaseName);

        _handler = new(mapper, dbContext);
    }

    [Fact]
    public async Task Handle_ValidTrainingDetails_CreatesTrainingRecord()
    {
        var command = new CreateTrainingCommandFaker().Generate();
        var training = await _handler.Handle(command, default);

        training.Should().NotBeNull();
        training.Id.Should().NotBe(Guid.Empty);
        training.Name.Should().Be(command.Name);
        training.Description.Should().Be(command.Description);
        training.PlannedAt.Should().Be(command.PlannedAt);
        training.Participants.Should().BeEmpty();
    }
}