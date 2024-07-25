using Ehrms.Contracts.Project;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class ProjectDeletedEventFaker : Faker<ProjectDeletedEvent>
{
	public ProjectDeletedEventFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
	}

	public ProjectDeletedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}