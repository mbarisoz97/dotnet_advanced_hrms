using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Command;

internal class UpdateSkillCommandFaker : Faker<UpdateSkillCommand>
{
	internal UpdateSkillCommandFaker()
    {
		RuleFor(x => x.Id, f => f.Random.Guid());
		RuleFor(x => x.Name, f => f.Name.Random.Words(3));
	}

	internal UpdateSkillCommandFaker WithId(Guid id)
	{
		RuleFor(x => x.Id, id);
		return this;
	}

	internal UpdateSkillCommandFaker WithName(string name)
	{
		RuleFor(x => x.Name, name);
		return this;
	}
}