namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training.Commands;

public class CreateTrainingCommandHandlerTests
{
    private const string DatabaseName = "CreateTrainingCommandHandlerTestDb";
    private readonly CreateTrainingCommandHandler _handler;

    public CreateTrainingCommandHandlerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);

        _handler = new(mapper, dbContext);
    }

    [Fact]
    public async Task Handle_ValidTrainingDetails_CreatesTrainingRecord()
    {
        var command = new CreateTrainingCommandFaker().Generate();
        var training = await _handler.Handle(command, default);

        training.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task Handle_TrainingEndDateIsCloserThanStartDate_ThrowsValidationException()
    {
        var command = new CreateTrainingCommandFaker()
            .WithStartDate(DateTime.UtcNow.AddHours(1))
            .WithEndDate(DateTime.UtcNow)
            .Generate();

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
        {
            await _handler.Handle(command, default);
        });
    }
}