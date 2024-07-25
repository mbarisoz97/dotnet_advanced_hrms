namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class TrainingFaker : Faker<Training>
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