using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class SkillFaker : Faker<Skill>
{
	public SkillFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Words(2));
	}
}