using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Models;

internal class SkillFaker : Faker<Skill>
{
	public SkillFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Name.Random.Words(3));
	}
}