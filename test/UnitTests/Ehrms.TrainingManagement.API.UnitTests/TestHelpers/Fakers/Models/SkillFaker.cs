namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class SkillFaker : Faker<Skill>
{
	public SkillFaker()
	{
		RuleFor(x => x.Id, f => f.Random.Guid());
		RuleFor(x => x.SkillName, f => f.Random.Word());
	}
}