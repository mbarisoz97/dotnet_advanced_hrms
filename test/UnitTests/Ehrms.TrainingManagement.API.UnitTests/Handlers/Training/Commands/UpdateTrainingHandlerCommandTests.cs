namespace Ehrms.TrainingManagement.API.UnitTests.Handlers.Training;

public class UpdateTrainingHandlerCommandTests
{
    private const string DatabaseName = "UpdateTrainingHandlerCommandTestsDb";
    private readonly UpdateTrainingCommandHandler _handler;

    public UpdateTrainingHandlerCommandTests()
    {
        var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
        var mapper = MapperFactory.CreateWithExistingProfiles();

        _handler = new(mapper, dbContext);
    }

    [Fact]
    public async Task Handle_NonExistingTraining_ThrowsTrainingNotFoundException()
    {
        await Assert.ThrowsAsync<TrainingNotFoundException>(async () =>
        {
            await _handler.Handle(new UpdateTrainingCommand(), default);
        });
    }

    [Fact]
    public async Task Handle_ExistingTraining_UpdatesTrainingRecord()
    {
        var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
        var employeeFaker = new EmployeeFaker();
        var employeeList = new List<Employee>
        {
            employeeFaker.Generate(),
            employeeFaker.Generate()
        };
        await dbContext.AddRangeAsync(employeeList);
        var training = new TrainingFaker().Generate();
        await dbContext.AddAsync(training);
        await dbContext.SaveChangesAsync();

        var command = new UpdateTrainingCommandFaker()
            .WithId(training.Id) 
            .Generate();
        command.Participants = employeeList.Select(x => x.Id).ToList();

        var updatedTraining = await _handler.Handle(command, default);

        updatedTraining.Name.Should().Be(command.Name);
        updatedTraining.PlannedAt.Should().Be(command.PlannedAt);
        updatedTraining.Description.Should().Be(command.Description);
        updatedTraining.Participants.Should().HaveCount(employeeList.Count);
    }
}