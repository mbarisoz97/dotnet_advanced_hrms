namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Skill;

public class CreateSkillCommandFaker : Faker<CreateSkillCommand>
{
    public CreateSkillCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.Words(3));
    }

    public CreateSkillCommandFaker WithName(string name)
    {
        RuleFor(x => x.Name, name);
        return this;
    }
}
