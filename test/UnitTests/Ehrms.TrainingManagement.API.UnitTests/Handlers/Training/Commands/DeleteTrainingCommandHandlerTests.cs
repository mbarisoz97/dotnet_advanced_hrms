namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training;

public class DeleteTrainingCommandHandlerTests
{
    private const string DatabaseName = "DeleteTrainingCommandHandlerTestDb";
    private readonly DeleteTrainingCommandHandler _handler;

    public DeleteTrainingCommandHandlerTests()
    {
        var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
        _handler = new(dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingTraining_ThrowsTrainingNotFoundException()
    {
        await Assert.ThrowsAsync<TrainingNotFoundException>(async () =>
        {
            await _handler.Handle(new DeleteTrainingCommand(), default);
        });
    }

    [Fact]
    public async Task Handle_ExistingTrainig_RemovesTrainingRecord()
    {
        var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
        var training = new TrainingFaker().Generate();
        await dbContext.AddAsync(training);
        await dbContext.SaveChangesAsync();

        await _handler.Handle(new DeleteTrainingCommand { Id = training.Id}, default);

        dbContext.Trainings
            .Should()
            .HaveCount(0);
    }
}