namespace Ehrms.Training.TestHelpers.Fakers.Models;

public sealed class TrainingFaker : Faker<TrainingManagement.API.Database.Models.Training>
{
	public TrainingFaker()
	{
		RuleFor(e => e.Name, f => f.Random.Word());
		RuleFor(e => e.PlannedAt, f => f.Date.Future());
		RuleFor(e => e.Description, f => f.Random.Words());
	}

	public TrainingFaker WithParticipants(ICollection<Employee> participants)
	{
		RuleFor(e => e.Participants, participants);
		return this;
	}
}