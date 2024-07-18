namespace Ehrms.ProjectManagemet.TestHelpers.Fakers.Model;

public class SkillFaker : Faker<Skill>
{
	public SkillFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Words(2));
	}
}
