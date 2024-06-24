namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training.Queries;

public class GetTrainingByIdQueryHandlerTests
{
    private const string DatabaseName = "GetTrainingByIdQueryHandlerTestDb";
    private readonly GetTrainingByIdQueryHandler _handler;

    public GetTrainingByIdQueryHandlerTests()
    {
        var dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase(DatabaseName);
        _handler = new(dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingTraining_ThrowsTrainingNotFoundException()
    {
        await Assert.ThrowsAsync<TrainingNotFoundException>(async () =>
        {
            await _handler.Handle(new GetTrainingByIdQuery(), default);
        });
    }

    [Fact]
    public async Task Handle_ExistingTraining_ReturnsExpectedTrainingRecord()
    {
        var dbContext = CustomDbContextFactory.CreateWithInMemoryDatabase(DatabaseName);
        var training = new TrainingFaker().Generate();
        await dbContext.AddAsync(training);
        await dbContext.SaveChangesAsync();

        var query = new GetTrainingByIdQuery { Id = training.Id };
        var returnedTraining = await _handler.Handle(query, default);

        returnedTraining.Id.Should().Be(training.Id);
        returnedTraining.Name.Should().Be(training.Name);
        returnedTraining.Description.Should().Be(training.Description);
        returnedTraining.PlannedAt.Should().Be(training.PlannedAt);
    }
}
