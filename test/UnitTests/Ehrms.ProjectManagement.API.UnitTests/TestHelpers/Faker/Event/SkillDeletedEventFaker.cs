using Ehrms.Contracts.Skill;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Event;

internal class SkillDeletedEventFaker : Faker<SkillDeletedEvent>
{
    public SkillDeletedEventFaker()
    {
		RuleFor(e => e.Id, f => f.Random.Guid());
	}
	public SkillDeletedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}