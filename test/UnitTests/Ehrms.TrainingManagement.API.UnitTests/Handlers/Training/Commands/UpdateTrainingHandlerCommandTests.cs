using Ehrms.TrainingManagement.API.Database.Models;

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
	public async Task Handle_EmptyParticipantList_UpdatesTrainingRecord()
	{
		var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
		var employeeList = new EmployeeFaker().Generate(2);
		await dbContext.AddRangeAsync(employeeList);
		
		var training = new TrainingFaker()
			.WithParticipants(employeeList)
			.Generate();
		await dbContext.AddAsync(training);

		await dbContext.SaveChangesAsync();

		var command = new UpdateTrainingCommandFaker()
			.WithId(training.Id)
			.WithParticipants(Array.Empty<Employee>())
			.Generate();

		var updatedTraining = await _handler.Handle(command, default);

		updatedTraining.Name.Should().Be(command.Name);
		updatedTraining.PlannedAt.Should().Be(command.PlannedAt);
		updatedTraining.Description.Should().Be(command.Description);
		updatedTraining.Participants.Should().HaveCount(0);
	}

	[Fact]
	public async Task Handle_NewParticipant_UpdatesTrainingRecord()
	{
		var dbContext = TestDbContextFactory.CreateDbContext(DatabaseName);
		var employeeList = new EmployeeFaker().Generate(2);
		await dbContext.AddRangeAsync(employeeList);

		var training = new TrainingFaker()
			.WithParticipants(Array.Empty<Employee>())
			.Generate();
		await dbContext.AddAsync(training);

		await dbContext.SaveChangesAsync();

		var command = new UpdateTrainingCommandFaker()
			.WithId(training.Id)
			.WithParticipants(employeeList)
			.Generate();

		var updatedTraining = await _handler.Handle(command, default);

		updatedTraining.Name.Should().Be(command.Name);
		updatedTraining.PlannedAt.Should().Be(command.PlannedAt);
		updatedTraining.Description.Should().Be(command.Description);
		updatedTraining.Participants.Should().HaveCount(employeeList.Count);
	}
}