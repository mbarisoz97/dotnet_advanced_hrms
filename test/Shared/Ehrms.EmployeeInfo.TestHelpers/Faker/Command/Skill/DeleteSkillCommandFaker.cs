namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Skill;

public class DeleteSkillCommandFaker : Faker<DeleteSkillCommand>
{
    public DeleteSkillCommandFaker()
    {
        RuleFor(p => p.Id, f => f.Random.Guid());
    }

    public DeleteSkillCommandFaker WithId(Guid id)
    {
        RuleFor(p => p.Id, id);
        return this;
    }
}