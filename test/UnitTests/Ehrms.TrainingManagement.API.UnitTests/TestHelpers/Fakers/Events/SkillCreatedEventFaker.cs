using Ehrms.Contracts.Skill;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class SkillCreatedEventFaker : Faker<SkillCreatedEvent>
{
    public SkillCreatedEventFaker()
    {
		RuleFor(e=> e.Id , f => f.Random.Guid());
		RuleFor(e=> e.Name, f => f.Random.Words(2));
	}

	public SkillCreatedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}