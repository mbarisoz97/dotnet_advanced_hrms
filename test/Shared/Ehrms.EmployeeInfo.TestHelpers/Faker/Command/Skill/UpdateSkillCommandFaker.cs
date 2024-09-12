namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Skill;

public class UpdateSkillCommandFaker : Faker<UpdateSkillCommand>
{
    public UpdateSkillCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Name.Random.Words(3));
    }

    public UpdateSkillCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public UpdateSkillCommandFaker WithName(string name)
    {
        RuleFor(x => x.Name, name);
        return this;
    }
}