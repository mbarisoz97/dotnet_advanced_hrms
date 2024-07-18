using Ehrms.Contracts.Skill;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Event;

internal class SkillUpdatedEventFaker : Faker<SkillUpdatedEvent>
{
	public SkillUpdatedEventFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Words(3));
	}
	public SkillUpdatedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}