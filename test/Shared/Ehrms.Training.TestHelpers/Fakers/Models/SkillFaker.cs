namespace Ehrms.Training.TestHelpers.Fakers.Models;

public sealed class SkillFaker : Faker<Skill>
{
    public SkillFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Random.Word());
    }
}