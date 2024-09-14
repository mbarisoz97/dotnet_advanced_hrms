using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Model;

public class SkillFaker : Faker<Skill>
{
    public SkillFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.Name, f => f.Name.Random.Words(3));
    }
}